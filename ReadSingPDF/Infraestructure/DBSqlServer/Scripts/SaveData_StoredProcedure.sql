if  Object_id(N'DBO.SaveData','P') IS NOT NULL 
begin 
 drop Procedure [DBO].[SaveData];
end;
go

create procedure dbo.SaveData
	@name		nvarchar(200) = '',
	@email		nvarchar(300) = '',

	@document	nvarchar(max) = '',
	@path		nvarchar(max) = ''
as

set nocount on
set xact_abort on


if IsNull(@name, '') = ''
begin
	raiserror('Debe ingresar el nombre de l usuario.', 16, 1)
	return
end

if IsNull(@email, '') = ''
begin
	raiserror('Debe ingresar el email del usuario.', 16, 1)
	return
end

if IsNull(@document, '') = ''
begin
	raiserror('Debe ingresar el nombre del documento.', 16, 1)
	return
end

if IsNull(@path, '') = ''
begin
	raiserror('Debe ingresar la ruta del archivo.', 16, 1)
	return
end

declare @id_user int

select @id_user = Id from UserPDf where Email = @email

if IsNull(@id_user, 0) = 0
begin
	insert into UserPDF (Name, Email)
	values (@name, @email)

	set @id_user = Scope_Identity()
end

insert into DocumentPDF (Id_USer, Name, Path)
values (@id_user, @document, @path)


