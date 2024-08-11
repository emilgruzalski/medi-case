output "vpc_id" {
  description = "ID of the VPC."
  value       = module.vpc.vpc_id
}

output "database_endpoint" {
  description = "Address of the PostgreSQL database."
  value       = aws_db_instance.main.address
}

output "database_secret_arn" {
  description = "Secrets Manager secret with the database credentials."
  value       = aws_db_instance.main.master_user_secret[0].secret_arn
}

output "files_bucket" {
  description = "S3 bucket with uploaded files."
  value       = aws_s3_bucket.files.bucket
}

output "cognito_user_pool_id" {
  description = "ID of the Cognito user pool."
  value       = aws_cognito_user_pool.main.id
}

output "cognito_client_id" {
  description = "ID of the Cognito app client used by the frontend."
  value       = aws_cognito_user_pool_client.web.id
}

output "ecr_repository_url" {
  description = "ECR repository for API images."
  value       = aws_ecr_repository.api.repository_url
}

output "ecs_cluster_name" {
  description = "Name of the ECS cluster."
  value       = aws_ecs_cluster.main.name
}

output "ecs_service_name" {
  description = "Name of the API service."
  value       = aws_ecs_service.api.name
}

output "ecs_task_definition_family" {
  description = "Family of the API task definition."
  value       = aws_ecs_task_definition.api.family
}

output "api_subnet_ids" {
  description = "Private subnets of the API tasks, also used by the migration task."
  value       = module.vpc.private_subnets
}

output "api_security_group_id" {
  description = "Security group of the API tasks."
  value       = aws_security_group.api.id
}
