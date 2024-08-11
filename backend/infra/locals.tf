locals {
  name = "${var.project}-${var.environment}"
  azs  = slice(data.aws_availability_zones.available.names, 0, var.availability_zone_count)

  # Subnet ranges: public 10.20.0.x/24, private 10.20.10.x/24, database 10.20.20.x/24.
  subnet_newbits         = 8
  private_subnet_offset  = 10
  database_subnet_offset = 20
}

locals {
  api_container_name = "api"
  api_container_port = 8080
  http_port          = 80
  postgres_port      = 5432
  health_check_path  = "/health"

  # CloudFront sends this header to the load balancer, other requests are rejected.
  origin_verify_header = "X-Origin-Verify"

  ecr_images_to_keep = 20
}
