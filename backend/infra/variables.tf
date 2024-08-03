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
