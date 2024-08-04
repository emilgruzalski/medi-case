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
