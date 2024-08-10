variable "project" {
  description = "Project name used in resource names."
  type        = string
  default     = "medicase"
}

variable "environment" {
  description = "Environment name, e.g. prod or staging."
  type        = string
  default     = "prod"
}

variable "region" {
  description = "AWS region for all resources."
  type        = string
  default     = "eu-central-1"
}

variable "vpc_cidr" {
  description = "CIDR block of the VPC."
  type        = string
  default     = "10.20.0.0/16"
}

variable "availability_zone_count" {
  description = "Number of availability zones used by the VPC."
  type        = number
  default     = 2
}

variable "db_instance_class" {
  description = "Instance class of the PostgreSQL database."
  type        = string
  default     = "db.t4g.micro"
}

variable "db_allocated_storage_gb" {
  description = "Initial storage of the database in GB."
  type        = number
  default     = 20
}

variable "db_max_allocated_storage_gb" {
  description = "Upper limit for storage autoscaling in GB."
  type        = number
  default     = 100
}

variable "db_backup_retention_days" {
  description = "Number of days automated backups are kept."
  type        = number
  default     = 7
}

variable "cognito_password_min_length" {
  description = "Minimum password length in the user pool."
  type        = number
  default     = 8
}

variable "cognito_temporary_password_days" {
  description = "Validity of temporary passwords sent with invitations."
  type        = number
  default     = 7
}
