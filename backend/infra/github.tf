locals {
  github_oidc_url = "https://token.actions.githubusercontent.com"
  # Thumbprints of the GitHub Actions OIDC certificates.
  github_oidc_thumbprints = [
    "6938fd4d98bab03faadb97b34396831e3780aea1",
    "1c58a3a8518e8759bf075b76b750d4f2df264fcd",
  ]
}

resource "aws_iam_openid_connect_provider" "github" {
  url             = local.github_oidc_url
  client_id_list  = ["sts.amazonaws.com"]
  thumbprint_list = local.github_oidc_thumbprints
}

data "aws_iam_policy_document" "github_assume" {
  for_each = {
    backend  = var.github_backend_repository
    frontend = var.github_frontend_repository
  }

  statement {
    actions = ["sts:AssumeRoleWithWebIdentity"]

    principals {
      type        = "Federated"
      identifiers = [aws_iam_openid_connect_provider.github.arn]
    }

    condition {
      test     = "StringEquals"
      variable = "token.actions.githubusercontent.com:aud"
      values   = ["sts.amazonaws.com"]
    }

    condition {
      test     = "StringEquals"
      variable = "token.actions.githubusercontent.com:sub"
      values   = ["repo:${each.value}:ref:refs/heads/${var.github_deploy_branch}"]
    }
  }
}

resource "aws_iam_role" "deploy_backend" {
  name               = "${local.name}-deploy-backend"
  assume_role_policy = data.aws_iam_policy_document.github_assume["backend"].json
}

data "aws_iam_policy_document" "deploy_backend" {
  statement {
    actions   = ["ecr:GetAuthorizationToken"]
    resources = ["*"]
  }

  statement {
    actions = [
      "ecr:BatchCheckLayerAvailability",
      "ecr:BatchGetImage",
      "ecr:CompleteLayerUpload",
      "ecr:InitiateLayerUpload",
      "ecr:PutImage",
      "ecr:UploadLayerPart",
    ]
    resources = [aws_ecr_repository.api.arn]
  }

  statement {
    # These actions do not support resource-level permissions.
    actions   = ["ecs:DescribeTaskDefinition", "ecs:RegisterTaskDefinition", "ecs:DescribeTasks"]
    resources = ["*"]
  }

  statement {
    actions   = ["ecs:UpdateService", "ecs:DescribeServices"]
    resources = [aws_ecs_service.api.id]
  }

  statement {
    actions   = ["ecs:RunTask"]
    resources = ["arn:aws:ecs:${var.region}:${data.aws_caller_identity.current.account_id}:task-definition/${aws_ecs_task_definition.api.family}:*"]
  }

  statement {
    actions   = ["iam:PassRole"]
    resources = [aws_iam_role.api_execution.arn, aws_iam_role.api_task.arn]
  }
}

resource "aws_iam_role_policy" "deploy_backend" {
  name   = "deploy"
  role   = aws_iam_role.deploy_backend.id
  policy = data.aws_iam_policy_document.deploy_backend.json
}

resource "aws_iam_role" "deploy_frontend" {
  name               = "${local.name}-deploy-frontend"
  assume_role_policy = data.aws_iam_policy_document.github_assume["frontend"].json
}

data "aws_iam_policy_document" "deploy_frontend" {
  statement {
    actions   = ["s3:ListBucket"]
    resources = [aws_s3_bucket.frontend.arn]
  }

  statement {
    actions   = ["s3:PutObject", "s3:DeleteObject"]
    resources = ["${aws_s3_bucket.frontend.arn}/*"]
  }

  statement {
    actions   = ["cloudfront:CreateInvalidation"]
    resources = [aws_cloudfront_distribution.main.arn]
  }
}

resource "aws_iam_role_policy" "deploy_frontend" {
  name   = "deploy"
  role   = aws_iam_role.deploy_frontend.id
  policy = data.aws_iam_policy_document.deploy_frontend.json
}
