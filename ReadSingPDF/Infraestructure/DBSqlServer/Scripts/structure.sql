

create database SignPDF;
use SignPDF;

create table UserPDF (
	Id int Identity(1, 1) primary key,
	Name varchar(200) not null,
	Email varchar(300) not null
)

create table DocumentPDF (
	Id int Identity(1, 1) primary key,
	Id_User int null,
	Name varchar(max) not null,
	Path varchar(max) not null

	foreign key (Id_User) references UserPDF (Id)
)
