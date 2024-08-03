module "vpc" {
  source  = "terraform-aws-modules/vpc/aws"
  version = "5.10.0"

  name = local.name
  cidr = var.vpc_cidr
  azs  = local.azs

  public_subnets   = [for i, _ in local.azs : cidrsubnet(var.vpc_cidr, local.subnet_newbits, i)]
  private_subnets  = [for i, _ in local.azs : cidrsubnet(var.vpc_cidr, local.subnet_newbits, i + local.private_subnet_offset)]
  database_subnets = [for i, _ in local.azs : cidrsubnet(var.vpc_cidr, local.subnet_newbits, i + local.database_subnet_offset)]

  # One NAT gateway is enough for this traffic and keeps the cost low.
  enable_nat_gateway           = true
  single_nat_gateway           = true
  create_database_subnet_group = true
  enable_dns_hostnames         = true
}
