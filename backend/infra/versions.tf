terraform {
  required_version = ">= 1.9"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.61.0"
    }
    random = {
      source  = "hashicorp/random"
      version = "3.6.2"
    }
  }

  # Bucket, key and lock table are passed with: terraform init -backend-config=backend.hcl
  backend "s3" {}
}
