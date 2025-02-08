
create table global_id_master(
                                 global_id int IDENTITY(1,1) not null,
                                 object_type varchar(50) not null,
                                 audit_create_date datetime not null,
                                 audit_create_user varchar(50) not null,
                                 audit_update_date datetime null,
                                 audit_update_user varchar(50) null,
                                 constraint PK_global_id_master PRIMARY KEY CLUSTERED (
                                     global_id asc
                                     )
)






create table codification(
                             global_id int not null,
                             codification_code varchar(50) not null,
                             version_id int not null,
                             code_txt varchar(50) null,
                             code_num int null,
                             audit_create_date datetime not null,
                             audit_create_user varchar(50) not null,
                             audit_update_date datetime null,
                             audit_update_user varchar(50) null,
                             constraint PK_codification primary key clustered (
                                 global_id ASC,
                                 codification_code ASC,
                                 version_id ASC
                                 )
)

    go

alter table codification with check add constraint FK_codification_global_id_master foreign key global_id references global_id_master




create table company_id_master(
                                  company_id int not null,
                                  company_type varchar(50) not null,
                                  audit_create_date datetime not null,
                                  audit_create_user varchar(50) not null,
                                  audit_update_date datetime null,
                                  audit_update_user varchar(50) null,
                                  constraint PK_company primary key clustered (
                                      company_id asc
                                      )
)

--FK globalid 



create table company_data(
                             company_id int not null,
                             value_date date not null,
                             data_code varchar(50) not null,
                             data_value_txt varchar(5000) not null,
                             data_value_int bigint null,
                             data_value_date date null,
                             data_value_bit bit null,
                             audit_create_date datetime not null,
                             audit_create_user varchar(50) not null,
                             audit_update_date datetime null,
                             audit_update_user varchar(50) null,
                             constraint PK_company_data primary key clustered (
                                 company_id asc,
                                 value_date asc,
                                 data_code asc
                                 )
)

--PK company id master
