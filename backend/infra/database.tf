resource "aws_security_group" "database" {
  name        = "${local.name}-database"
  description = "PostgreSQL database, reachable only from the API."
  vpc_id      = module.vpc.vpc_id
}

resource "aws_db_instance" "main" {
  identifier     = local.name
  engine         = "postgres"
  engine_version = "16.3"
  instance_class = var.db_instance_class

  allocated_storage     = var.db_allocated_storage_gb
  max_allocated_storage = var.db_max_allocated_storage_gb
  storage_type          = "gp3"
  storage_encrypted     = true

  # Both application databases (medicase and medicase_published) are created by EF Core migrations.
  db_name  = "medicase"
  username = "medicase"
  # The password is generated and rotated by RDS and kept in Secrets Manager.
  manage_master_user_password = true

  db_subnet_group_name   = module.vpc.database_subnet_group_name
  vpc_security_group_ids = [aws_security_group.database.id]

  backup_retention_period   = var.db_backup_retention_days
  deletion_protection       = true
  skip_final_snapshot       = false
  final_snapshot_identifier = "${local.name}-final"
}
