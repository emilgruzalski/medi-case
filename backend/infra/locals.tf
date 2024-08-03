locals {
  name = "${var.project}-${var.environment}"
  azs  = slice(data.aws_availability_zones.available.names, 0, var.availability_zone_count)

  # Subnet ranges: public 10.20.0.x/24, private 10.20.10.x/24, database 10.20.20.x/24.
  subnet_newbits         = 8
  private_subnet_offset  = 10
  database_subnet_offset = 20
}
