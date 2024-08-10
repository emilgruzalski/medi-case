locals {
  # Cognito groups that map to application roles. Users without a group are learners.
  role_groups = ["Admin", "Moderator"]

  cognito_token_validity = {
    access_hours = 1
    id_hours     = 1
    refresh_days = 30
  }

  cognito_name_max_length = 100
}

resource "aws_cognito_user_pool" "main" {
  name                     = local.name
  username_attributes      = ["email"]
  auto_verified_attributes = ["email"]
  deletion_protection      = "ACTIVE"

  password_policy {
    minimum_length                   = var.cognito_password_min_length
    require_lowercase                = true
    require_uppercase                = true
    require_numbers                  = true
    require_symbols                  = true
    temporary_password_validity_days = var.cognito_temporary_password_days
  }

  account_recovery_setting {
    recovery_mechanism {
      name     = "verified_email"
      priority = 1
    }
  }

  schema {
    name                = "given_name"
    attribute_data_type = "String"
    mutable             = true
    required            = true

    string_attribute_constraints {
      min_length = 1
      max_length = local.cognito_name_max_length
    }
  }

  schema {
    name                = "family_name"
    attribute_data_type = "String"
    mutable             = true
    required            = true

    string_attribute_constraints {
      min_length = 1
      max_length = local.cognito_name_max_length
    }
  }

  verification_message_template {
    default_email_option = "CONFIRM_WITH_CODE"
    email_subject        = "MediCase – kod weryfikacyjny"
    email_message        = "Twój kod weryfikacyjny do MediCase: {####}"
  }

  admin_create_user_config {
    allow_admin_create_user_only = false

    invite_message_template {
      email_subject = "Zaproszenie do MediCase"
      email_message = "Zostało dla Ciebie utworzone konto w MediCase. Login: {username}, hasło tymczasowe: {####}"
      sms_message   = "MediCase – login: {username}, hasło tymczasowe: {####}"
    }
  }
}

resource "aws_cognito_user_pool_client" "web" {
  name         = "${local.name}-web"
  user_pool_id = aws_cognito_user_pool.main.id

  # The single page app cannot keep a secret.
  generate_secret = false
  explicit_auth_flows = [
    "ALLOW_USER_PASSWORD_AUTH",
    "ALLOW_USER_SRP_AUTH",
    "ALLOW_REFRESH_TOKEN_AUTH",
  ]
  prevent_user_existence_errors = "ENABLED"
  enable_token_revocation       = true

  access_token_validity  = local.cognito_token_validity.access_hours
  id_token_validity      = local.cognito_token_validity.id_hours
  refresh_token_validity = local.cognito_token_validity.refresh_days

  token_validity_units {
    access_token  = "hours"
    id_token      = "hours"
    refresh_token = "days"
  }

  read_attributes  = ["email", "email_verified", "given_name", "family_name"]
  write_attributes = ["email", "given_name", "family_name"]
}

resource "aws_cognito_user_group" "roles" {
  for_each = toset(local.role_groups)

  name         = each.value
  user_pool_id = aws_cognito_user_pool.main.id
}
