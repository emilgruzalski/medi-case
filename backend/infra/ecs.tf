resource "aws_cloudwatch_log_group" "api" {
  name              = "/ecs/${local.name}-api"
  retention_in_days = var.log_retention_days
}

resource "aws_ecs_cluster" "main" {
  name = local.name

  setting {
    name  = "containerInsights"
    value = "enabled"
  }
}

resource "aws_security_group" "api" {
  name        = "${local.name}-api"
  description = "API tasks, reachable only from the load balancer."
  vpc_id      = module.vpc.vpc_id
}

resource "aws_vpc_security_group_ingress_rule" "api_from_alb" {
  security_group_id            = aws_security_group.api.id
  referenced_security_group_id = aws_security_group.alb.id
  ip_protocol                  = "tcp"
  from_port                    = local.api_container_port
  to_port                      = local.api_container_port
}

resource "aws_vpc_security_group_egress_rule" "api_all" {
  security_group_id = aws_security_group.api.id
  cidr_ipv4         = "0.0.0.0/0"
  ip_protocol       = "-1"
}

resource "aws_vpc_security_group_ingress_rule" "database_from_api" {
  security_group_id            = aws_security_group.database.id
  referenced_security_group_id = aws_security_group.api.id
  ip_protocol                  = "tcp"
  from_port                    = local.postgres_port
  to_port                      = local.postgres_port
}

locals {
  database_connection = "Host=${aws_db_instance.main.address};Port=${local.postgres_port};Username=${aws_db_instance.main.username}"
}

resource "aws_ecs_task_definition" "api" {
  family                   = "${local.name}-api"
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  cpu                      = var.api_cpu
  memory                   = var.api_memory
  execution_role_arn       = aws_iam_role.api_execution.arn
  task_role_arn            = aws_iam_role.api_task.arn

  container_definitions = jsonencode([{
    name         = local.api_container_name
    image        = "${aws_ecr_repository.api.repository_url}:${var.api_image_tag}"
    essential    = true
    portMappings = [{ containerPort = local.api_container_port, protocol = "tcp" }]

    environment = [
      { name = "ASPNETCORE_ENVIRONMENT", value = "Production" },
      { name = "ConnectionStrings__MedicalDatabase", value = "${local.database_connection};Database=medicase" },
      { name = "ConnectionStrings__PublishedDatabase", value = "${local.database_connection};Database=medicase_published" },
      # Migrations run as a separate task before each deployment.
      { name = "Database__MigrateOnStartup", value = "false" },
      { name = "FileStorage__Provider", value = "S3" },
      { name = "FileStorage__BucketName", value = aws_s3_bucket.files.bucket },
      { name = "Cognito__Region", value = var.region },
      { name = "Cognito__UserPoolId", value = aws_cognito_user_pool.main.id },
      { name = "Cognito__ClientId", value = aws_cognito_user_pool_client.web.id },
      { name = "AWS__Region", value = var.region },
    ]

    secrets = [
      { name = "Database__Password", valueFrom = "${aws_db_instance.main.master_user_secret[0].secret_arn}:password::" },
    ]

    logConfiguration = {
      logDriver = "awslogs"
      options = {
        awslogs-group         = aws_cloudwatch_log_group.api.name
        awslogs-region        = var.region
        awslogs-stream-prefix = local.api_container_name
      }
    }
  }])
}

resource "aws_ecs_service" "api" {
  name                              = "${local.name}-api"
  cluster                           = aws_ecs_cluster.main.id
  task_definition                   = aws_ecs_task_definition.api.arn
  desired_count                     = var.api_desired_count
  launch_type                       = "FARGATE"
  health_check_grace_period_seconds = 60

  network_configuration {
    subnets          = module.vpc.private_subnets
    security_groups  = [aws_security_group.api.id]
    assign_public_ip = false
  }

  load_balancer {
    target_group_arn = aws_lb_target_group.api.arn
    container_name   = local.api_container_name
    container_port   = local.api_container_port
  }

  deployment_circuit_breaker {
    enable   = true
    rollback = true
  }

  # New task definitions are registered by the deployment workflow.
  lifecycle {
    ignore_changes = [task_definition]
  }

  depends_on = [aws_lb_listener_rule.from_cloudfront]
}
