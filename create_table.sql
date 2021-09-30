create table dbo.admin_list_table (
    _id             integer identity(1,1) not null,
    _name           nvarchar(6) not null,
    _password       nvarchar(35) not null,
    constraint PK_admin_list_table primary key clustered(_id asc)
);

create table dbo.users_list_table (
    _id                               integer identity(1,1) not null,
    _name                             nchar(30) not null,
    _login                            integer not null,
    _password                         nchar(35) not null,
    _date_of_birth                    date not null,
    _home_path                        nchar(35) not null,
    _seria                            nchar(9) not null,
    constraint PK_users_list_table    primary key clustered(_id asc)
);
create table dbo.payment_list (
    _id                               integer identity(1,1) not null,
    _login                            integer not null,
    _date                             date not null,
    _summ                             float not null,
    constraint PK_payment_list    primary key clustered(_id asc)
);

create table dbo.users_application(
    _id                               integer identity(1,1) not null,
    _login                            nchar(30) not null,
    _user_gender                      nchar(3) not null,
    _user_age                         integer not null,
    _married                          nchar(10) not null,
    _nationality                      nchar(15) not null,
    _credit_summ_from_general_revenue nvarchar(10) not null,
    _credit_history                   integer not null,
    _arrearage_in_credit_history      integer not null,
    _credit_aim                       nvarchar(15)  not null,
    _credit_term                      integer not null,
    _status                           nvarchar(20),
    _balls                            integer,
    _results                          nvarchar(25),
    _is_payed                         integer,
    constraint PK_users_application    primary key clustered(_id asc)
);