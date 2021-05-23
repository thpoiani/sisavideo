-- DATABASE: sisavideo

CREATE DATABASE sisavideo;
go

USE sisavideo;
go

-- TABLES

CREATE TABLE funcionario(
	id integer IDENTITY NOT NULL,
	cpf	varchar (14) NOT NULL,
	rg varchar (12) NOT NULL,
	nome varchar (80) NOT NULL,
	email varchar (80) NOT NULL,
	data_nasc date NOT NULL,
	sexo char (1) NOT NULL,
	logradouro varchar (80) NOT NULL,
	numero integer,
	complemento varchar (20),
	cep varchar(9),
	cidade varchar (30) NOT NULL,
	cargo varchar (30),
	data_admissao date NOT NULL,
	data_demissao date,
);

ALTER TABLE funcionario
	ADD CONSTRAINT funcionario_id_pk PRIMARY KEY (id);

ALTER TABLE funcionario
	ADD CONSTRAINT funcionario_nome_unique UNIQUE (nome);
	
ALTER TABLE funcionario
	ADD CONSTRAINT funcionario_cpf_unique UNIQUE (cpf);
	
ALTER TABLE funcionario
	ADD CONSTRAINT funcionario_email_unique UNIQUE (email);

ALTER TABLE funcionario
	ADD CONSTRAINT funcionario_sexo_check CHECK (sexo is NULL OR sexo = 'm' OR sexo = 'M' OR sexo = 'f' OR sexo = 'F');

	
CREATE TABLE login(
	id integer NOT NULL,
	username varchar (16) NOT NULL,
	password varchar (32) NOT NULL,
	permissao char (1) NOT NULL
);

ALTER TABLE login
	ADD CONSTRAINT funcionario_login_fk FOREIGN KEY (id) REFERENCES funcionario(id)
	ON UPDATE CASCADE ON DELETE CASCADE;

ALTER TABLE login
	ADD CONSTRAINT login_username_unique UNIQUE (username);

	
CREATE TABLE telefone(
	id integer NOT NULL,
	telefone varchar (16) NOT NULL
);

ALTER TABLE telefone
	ADD CONSTRAINT funcionario_telefone_id_fk FOREIGN KEY (id) REFERENCES funcionario(id)
	ON UPDATE CASCADE ON DELETE CASCADE;

	
CREATE TABLE estagiario(
	id integer NOT NULL,
	polo integer,
	instituicao varchar (80) NOT NULL,
	curso varchar (80) NOT NULL,
	inicio_curso date,
	fim_curso date,
	data_contrato date,
	horario_entrada time,
	horario_saida time
);

ALTER TABLE estagiario
	ADD CONSTRAINT funcionario_estagiario_id_fk FOREIGN KEY (id) REFERENCES funcionario(id)
	ON UPDATE CASCADE ON DELETE CASCADE;
	
	
CREATE TABLE apontamento(
	funcionario integer NOT NULL,
	polo integer NOT NULL,
	apontamento_entrada datetime,
	apontamento_saida datetime,
	justificativa varchar(255)
);

ALTER TABLE apontamento
	ADD CONSTRAINT funcionario_apontamento_id_fk FOREIGN KEY (funcionario) REFERENCES funcionario(id)
	ON UPDATE CASCADE ON DELETE CASCADE;

	
CREATE TABLE polo(
	id integer IDENTITY NOT NULL,
	nome varchar (80) NOT NULL,
	endereco varchar (80) NOT NULL,
	cidade varchar (80) NOT NULL,
	capacidade integer NOT NULL,
	horario_abertura time NOT NULL,
	horario_fechamento time NOT NULL,
	email varchar (80) NOT NULL,
	senha varchar (32) NOT NULL,
	celular varchar (16) NOT NULL
);

ALTER TABLE polo
	ADD CONSTRAINT polo_id_pk PRIMARY KEY (id);
	
ALTER TABLE polo
	ADD CONSTRAINT polo_nome_unique UNIQUE (nome);

ALTER TABLE polo
	ADD CONSTRAINT polo_email_unique UNIQUE (email);
	
ALTER TABLE estagiario
	ADD CONSTRAINT polo_estagiario_id_fk FOREIGN KEY (polo) REFERENCES polo(id)
	ON UPDATE CASCADE ON DELETE SET NULL;

ALTER TABLE apontamento
	ADD CONSTRAINT polo_apontamento_id_fk FOREIGN KEY (polo) REFERENCES polo(id)
	ON UPDATE CASCADE ON DELETE CASCADE;
	
	
CREATE TABLE videoconferencia(
	codigo integer NOT NULL,
	titulo varchar (120) NOT NULL,
	data_ativacao date NOT NULL,
	horario_inicio time NOT NULL,
	horario_fim time NOT NULL,
	solicitante varchar (80) NOT NULL,
	secretaria varchar (80) NOT NULL,
	orgao varchar (80) NOT NULL,
	streaming varchar(80),
	estudio varchar (80) NOT NULL
);

ALTER TABLE videoconferencia
	ADD CONSTRAINT videoconferencia_codigo_pk PRIMARY KEY (codigo);
	

CREATE TABLE publicoalvo(
	codigo integer NOT NULL,
	publico varchar (80) NOT NULL
);	

ALTER TABLE publicoalvo
	ADD CONSTRAINT videoconferencia_publicoalvo_codigo_fk FOREIGN KEY (codigo) REFERENCES videoconferencia(codigo)
	ON DELETE CASCADE ON UPDATE CASCADE;

	
CREATE TABLE ativado(
	video integer NOT NULL,
	polo integer NOT NULL,
	contagem integer,
	horario_contagem datetime,
	estagiario integer
);

ALTER TABLE ativado
	ADD CONSTRAINT ativado_polo PRIMARY KEY (video, polo);

ALTER TABLE ativado
	ADD CONSTRAINT videoconferencia_ativado_codigo_fk FOREIGN KEY (video) REFERENCES videoconferencia(codigo)
	ON UPDATE CASCADE ON DELETE CASCADE;
	
ALTER TABLE ativado
	ADD CONSTRAINT estagiario_ativado_codigo_fk FOREIGN KEY (estagiario) REFERENCES funcionario(id)
	ON UPDATE CASCADE ON DELETE CASCADE;

ALTER TABLE ativado
	ADD CONSTRAINT polo_ativado_id_fk FOREIGN KEY (polo) REFERENCES polo(id)
	ON UPDATE CASCADE ON DELETE CASCADE;

	
CREATE TABLE conferencista(
	videoconferencia integer NOT NULL,
	cpf varchar (14) NOT NULL,
	nome varchar (80) NOT NULL
);
	
ALTER TABLE conferencista
	ADD CONSTRAINT conferencista_cpf_unique UNIQUE (cpf);
	
ALTER TABLE conferencista
	ADD CONSTRAINT videoconferencia_conferencista_codigo_fk FOREIGN KEY (videoconferencia) REFERENCES videoconferencia(codigo)
	ON UPDATE CASCADE ON DELETE CASCADE;

go

-- SCHEMAS

CREATE SCHEMA rh;
go

CREATE SCHEMA vc;
go

ALTER SCHEMA rh TRANSFER dbo.funcionario;
ALTER SCHEMA rh TRANSFER dbo.login;
ALTER SCHEMA rh TRANSFER dbo.telefone;
ALTER SCHEMA rh TRANSFER dbo.estagiario;
ALTER SCHEMA rh TRANSFER dbo.apontamento;
ALTER SCHEMA vc TRANSFER dbo.polo;
ALTER SCHEMA vc TRANSFER dbo.ativado;
ALTER SCHEMA vc TRANSFER dbo.videoconferencia;
ALTER SCHEMA vc TRANSFER dbo.conferencista;
ALTER SCHEMA vc TRANSFER dbo.publicoalvo;
go

-- USER dba

CREATE LOGIN dba WITH PASSWORD = 'root', DEFAULT_DATABASE=[sisavideo];
CREATE USER dba FOR LOGIN dba;
go

GRANT ALTER ANY USER TO dba WITH GRANT OPTION;
GRANT ALTER ANY ROLE TO dba WITH GRANT OPTION;

USE [master]
go
GRANT ALTER ANY LOGIN TO dba WITH GRANT OPTION;
go

USE [sisavideo]
go
ALTER AUTHORIZATION ON SCHEMA ::rh TO dba;
ALTER AUTHORIZATION ON SCHEMA ::vc TO dba;
go

GRANT EXECUTE TO dba;
GRANT CREATE FUNCTION TO dba;
GRANT CREATE PROCEDURE TO dba;
GRANT CREATE VIEW TO dba;


-- LOGOUT
-- LOGIN: dba

-- ROLE supervisor
CREATE ROLE supervisor;
GRANT SELECT ON SCHEMA :: rh TO supervisor;
GRANT SELECT ON SCHEMA :: vc TO supervisor;

GRANT SELECT, INSERT, UPDATE, DELETE ON rh.funcionario TO supervisor;
GRANT SELECT, INSERT, UPDATE, DELETE ON rh.telefone TO supervisor;
GRANT SELECT, INSERT, UPDATE, DELETE ON rh.estagiario TO supervisor;
GRANT SELECT, INSERT, UPDATE, DELETE ON rh.login TO supervisor;
GRANT SELECT, INSERT, UPDATE, DELETE ON rh.apontamento TO supervisor;
GRANT SELECT, INSERT, UPDATE, DELETE ON vc.polo TO supervisor;
GRANT SELECT, INSERT, UPDATE, DELETE ON vc.ativado TO supervisor;
GRANT SELECT, INSERT, UPDATE, DELETE ON vc.videoconferencia TO supervisor;
GRANT SELECT, INSERT, UPDATE, DELETE ON vc.conferencista TO supervisor;
GRANT SELECT, INSERT, UPDATE, DELETE ON vc.publicoalvo TO supervisor;

GRANT ALTER ANY USER TO supervisor WITH GRANT OPTION;
GRANT ALTER ANY ROLE TO supervisor WITH GRANT OPTION;
go

-- ROLE estagiario

CREATE ROLE estagiario;

GRANT SELECT ON SCHEMA :: rh TO estagiario;
GRANT SELECT ON SCHEMA :: vc TO estagiario;

GRANT SELECT, UPDATE ON rh.funcionario TO estagiario;
GRANT SELECT, UPDATE ON rh.telefone TO estagiario;
GRANT SELECT, UPDATE ON rh.estagiario TO estagiario;
GRANT SELECT, UPDATE ON rh.login TO estagiario;
GRANT SELECT, INSERT, UPDATE ON rh.apontamento TO estagiario;
GRANT SELECT ON vc.polo TO estagiario;
GRANT SELECT, INSERT, UPDATE ON vc.ativado TO estagiario;
GRANT SELECT ON vc.videoconferencia TO estagiario;
GRANT SELECT ON vc.conferencista TO estagiario;
GRANT SELECT ON vc.publicoalvo TO estagiario;
go

-- USER supervisor_login

CREATE LOGIN supervisor_login WITH PASSWORD = 'supervisor', DEFAULT_DATABASE=[sisavideo];
CREATE USER supervisor_login FOR LOGIN supervisor_login;
EXEC sp_addrolemember 'supervisor', 'supervisor_login'
go

USE [master]
go
GRANT ALTER ANY LOGIN TO supervisor_login WITH GRANT OPTION;
go
USE [sisavideo]
go

-- USER estagiario_login

CREATE LOGIN estagiario_login WITH PASSWORD = 'estagiario', DEFAULT_DATABASE=[sisavideo];
CREATE USER estagiario_login FOR LOGIN estagiario_login;
EXEC sp_addrolemember 'estagiario', 'estagiario_login'
go

-- FUNCTION MD5
CREATE FUNCTION rh.MD5 (@value varchar(32))
RETURNS varchar(32) AS
BEGIN
  RETURN LOWER(CONVERT(VARCHAR(32), hashbytes('MD5', @value), 2));
END;
go	   

GRANT EXECUTE ON rh.MD5 TO supervisor;
go

-- VIEW visualizar_funcionario

CREATE VIEW rh.visualizar_funcionario AS
SELECT Top (99) Percent f.nome, f.cpf, f.email, f.rg, UPPER(f.sexo) as sexo, 
	CONVERT(varchar(10), f.data_nasc, 103) as data_nasc, 
	f.logradouro, f.numero, f.complemento, f.cidade, f.cep, f.cargo, 
	CONVERT(varchar(10), f.data_admissao, 103) as data_admissao, 
	CONVERT(varchar(10), f.data_demissao, 103) as data_demissao, 
	t.telefone FROM rh.funcionario f INNER JOIN rh.telefone t
	ON f.id = t.id
	ORDER BY f.nome ASC;
go 

GRANT SELECT ON rh.visualizar_funcionario TO supervisor;
go

-- VIEW visualizar_estagiario

CREATE VIEW rh.visualizar_estagiario AS
SELECT Top (99) Percent f.nome AS nome, f.email AS email, p.nome AS polo,
	e.instituicao AS instituicao, e.curso AS curso, 
	CONVERT(varchar(10), e.inicio_curso, 103) AS inicio_curso,
	CONVERT(varchar(10), e.fim_curso, 103) AS fim_curso,
	CONVERT(varchar(10), e.data_contrato, 103) as data_contrato,
	CONVERT(varchar(5), e.horario_entrada, 108) as horario_entrada,
	CONVERT(varchar(5), e.horario_saida, 108) as horario_saida
		FROM rh.funcionario f INNER JOIN rh.estagiario e
			ON f.id = e.id INNER JOIN vc.polo p 
			ON e.polo = p.id;
go


GRANT SELECT ON rh.visualizar_estagiario TO supervisor;
GRANT SELECT ON rh.visualizar_estagiario TO estagiario;
go

-- PROCEDURE cadastrar_funcionario

CREATE PROCEDURE rh.cadastrar_funcionario @cpf varchar(14), @rg varchar(12), @nome varchar(80),
@email varchar (80), @data_nasc date, @sexo char (1), @logradouro varchar (80), 
@numero integer, @complemento varchar (20), @cep varchar(9), @cidade varchar (30), 
@cargo varchar (30), @data_admissao date, @telefone varchar(16), 
@username varchar(16), @password varchar(32), @check integer 
AS
INSERT INTO rh.funcionario
(cpf, rg, nome, email, data_nasc, sexo, logradouro, 
numero, complemento, cep, cidade, cargo, data_admissao)
	VALUES (@cpf, @rg, @nome, @email, @data_nasc, UPPER(@sexo), @logradouro, @numero, @complemento, 
	@cep, @cidade, @cargo, @data_admissao);

INSERT INTO rh.telefone (id, telefone)
	VALUES ((SELECT MAX(id) FROM rh.funcionario), @telefone);

DECLARE @sql NVARCHAR(250);

SET	@sql = 'CREATE LOGIN '+@username+' WITH PASSWORD = '+char(39)+''+@password+''+char(39)+', DEFAULT_DATABASE=[sisavideo]';
EXECUTE(@sql);

SET	@sql = 'CREATE USER '+@username+' FOR LOGIN '+@username;
EXECUTE(@sql);

IF @check = 1
BEGIN
	INSERT INTO rh.login (id, username, password, permissao)
		VALUES ((SELECT MAX(id) FROM rh.funcionario), @username, rh.MD5(@password), 'S');

	SET @sql = 'EXEC sp_addrolemember '+char(39)+'supervisor'+char(39)+', '+char(39)+''+@username+''+char(39)+'';
	EXECUTE(@sql);
	
	SET @sql =	'USE [master]; GRANT ALTER ANY LOGIN TO '+@username+' WITH GRANT OPTION';
	EXECUTE(@sql);
END
ELSE
BEGIN
	INSERT INTO rh.login (id, username, password, permissao)
		VALUES ((SELECT MAX(id) FROM rh.funcionario), @username, rh.MD5(@password), 'E');
		
	SET @sql = 'EXEC sp_addrolemember '+char(39)+'estagiario'+char(39)+', '+char(39)+''+@username+''+char(39)+'';
	EXECUTE(@sql);
END
GO	   

GRANT EXECUTE ON rh.cadastrar_funcionario TO supervisor;
go

-- PROCEDURE cadastrar_polo

CREATE PROCEDURE vc.cadastrar_polo @nome varchar(80), @endereco varchar(80), @cidade varchar(80), 
@capacidade integer, @horario_abertura varchar(5) , @horario_fechamento varchar(5), 
@email varchar(80), @senha varchar(32), @celular varchar(16)
AS
INSERT INTO vc.polo (nome, endereco, cidade, capacidade, horario_abertura, horario_fechamento, email, senha, celular)
	VALUES (@nome, @endereco, @cidade, @capacidade, @horario_abertura, @horario_fechamento, @email, @senha, @celular);
GO

GRANT EXECUTE ON vc.cadastrar_polo TO supervisor;
go

-- PROCEDURE cadastrar_videoconferencia

CREATE PROCEDURE vc.cadastrar_videoconferencia @titulo varchar(120), @codigo int, @data_ativacao date, 
@horario_inicio time, @horario_fim time, @solicitante varchar(80), @secretaria varchar(80), 
@orgao varchar(80), @streaming varchar(80), @estudio varchar(80)
AS
INSERT INTO vc.videoconferencia (titulo, codigo, data_ativacao, horario_inicio, horario_fim,
solicitante, secretaria, orgao, streaming, estudio)
	VALUES (@titulo, @codigo, @data_ativacao, @horario_inicio, @horario_fim, @solicitante, 
	@secretaria, @orgao, @streaming, @estudio);
GO

GRANT EXECUTE ON vc.cadastrar_videoconferencia TO supervisor;
go

-- PROCEDURE visualizar_funcionario_especifico

CREATE PROCEDURE rh.visualizar_funcionario_especifico @nome varchar(80)
AS
SELECT f.nome, f.cpf, f.email, f.rg, UPPER(f.sexo) as sexo, 
	   CONVERT(varchar(10), f.data_nasc, 103) as data_nasc, 
	   f.logradouro, f.numero, f.complemento, f.cidade, f.cep, f.cargo, 
	   CONVERT(varchar(10), f.data_admissao, 103) as data_admissao, 
	   CONVERT(varchar(10), f.data_demissao, 103) as data_demissao, 
	   t.telefone, l.permissao, l.username, l.password 
	   FROM rh.funcionario f INNER JOIN rh.telefone t
	   ON f.id = t.id INNER JOIN rh.login l ON l.id = f.id
	   WHERE f.nome = @nome;
GO

GRANT EXECUTE ON rh.visualizar_funcionario_especifico TO supervisor;
go

-- PROCEDURE visualizar_vioconferencia_especifica

CREATE PROCEDURE vc.visualizar_videoconferencia_especifica @codigo varchar(80)
AS
SELECT titulo,
	   CONVERT(varchar(10), data_ativacao, 103) as data_ativacao,
	   CONVERT(varchar(5), horario_inicio, 108) as horario_inicio, 
	   CONVERT(varchar(5), horario_fim, 108) as horario_fim, 
	   estudio, secretaria, orgao, solicitante, streaming 
	   FROM vc.videoconferencia WHERE codigo = @codigo;
GO

GRANT EXECUTE ON vc.visualizar_videoconferencia_especifica TO supervisor;
GO

-- PROCEDURE remover_funcionario

CREATE PROCEDURE rh.remover_funcionario @nome varchar(80) AS
	DECLARE @username NVARCHAR(16);
	DECLARE @sql NVARCHAR(250);
	
	SELECT @username = l.username FROM rh.funcionario f INNER JOIN rh.login l
		ON f.id = l.id
		WHERE f.nome = @nome;
	
	SET @sql = 'USE [sisavideo]; DROP USER ' + @username
	EXECUTE(@sql);
	
	SET @sql = 'USE [master]; DROP LOGIN ' + @username
	EXECUTE(@sql);	

	DELETE FROM rh.funcionario WHERE nome = @nome;
GO

GRANT EXECUTE ON rh.remover_funcionario TO supervisor;
go

-- PROCEDURE remover_estagiario
CREATE PROCEDURE rh.remover_estagiario @nome varchar(80) AS
	DECLARE @username NVARCHAR(16);
	
	DELETE FROM rh.estagiario WHERE id = 
	(SELECT f.id FROM rh.funcionario f INNER JOIN rh.estagiario e 
     ON f.id = e.id WHERE f.nome = @nome);
GO

GRANT EXECUTE ON rh.remover_estagiario TO supervisor;
go

-- PROCEDURE supervisor_to_estagiario

CREATE PROCEDURE rh.supervisor_to_estagiario @username varchar(16) AS
IF (SELECT permissao FROM rh.login WHERE username = @username) = 'S'
BEGIN
	UPDATE rh.login SET permissao = 'E' WHERE username = @username;

	DECLARE @sql NVARCHAR(250);
	SET @sql = 'EXEC sp_droprolemember '+char(39)+'supervisor'+char(39)+', '+char(39)+''+@username+''+char(39)+'';
	EXECUTE(@sql);
	
	SET @sql = 'EXEC sp_addrolemember '+char(39)+'estagiario'+char(39)+', '+char(39)+''+@username+''+char(39)+'';
	EXECUTE(@sql);	
END
GO

GRANT EXECUTE ON rh.supervisor_to_estagiario TO supervisor;
GO

-- PROCEDURE estagiario_to_supervisor

CREATE PROCEDURE rh.estagiario_to_supervisor @username varchar(16) AS
IF (SELECT permissao FROM rh.login WHERE username = @username) = 'E'
BEGIN
	UPDATE rh.login SET permissao = 'S' WHERE username = @username;
	
	DECLARE @sql NVARCHAR(250);
	SET @sql = 'EXEC sp_droprolemember '+char(39)+'estagiario'+char(39)+', '+char(39)+''+@username+''+char(39)+'';
	EXECUTE(@sql);
	
	SET @sql = 'EXEC sp_addrolemember '+char(39)+'supervisor'+char(39)+', '+char(39)+''+@username+''+char(39)+'';
	EXECUTE(@sql);
END
go

GRANT EXECUTE ON rh.estagiario_to_supervisor TO supervisor;
go

-- VIEW estagiario_nao_cadastrado

CREATE VIEW rh.estagiario_nao_cadastrado AS
SELECT Top (99) Percent f.nome FROM rh.funcionario f INNER JOIN rh.login l
ON f.id = l.id LEFT JOIN rh.estagiario e ON f.id = e.id 
WHERE l.permissao = 'E' AND e.id IS NULL
ORDER BY f.nome ASC;
go

GRANT SELECT ON rh.estagiario_nao_cadastrado TO supervisor;
go

-- CREATE PROCEDURE cadastrar_estagiario

CREATE PROCEDURE rh.cadastrar_estagiario @nome varchar(80), @polo varchar(80), 
@instituicao varchar(80), @curso varchar(80), @inicio_curso date, @fim_curso date,
@data_contrato date, @horario_entrada time, @horario_saida time
AS
INSERT INTO rh.estagiario 
(id, polo, instituicao, curso, inicio_curso, fim_curso, data_contrato, horario_entrada, horario_saida) VALUES 
((SELECT id FROM rh.funcionario WHERE nome = @nome), 
(SELECT id FROM vc.polo WHERE nome = @polo), 
@instituicao, @curso, @inicio_curso, @fim_curso, 
@data_contrato, @horario_entrada, @horario_saida);

DECLARE @username NVARCHAR(16);
SELECT @username = l.username FROM rh.funcionario f INNER JOIN rh.login l
	ON f.id = l.id
	WHERE f.nome = @nome;
go

GRANT EXECUTE ON rh.cadastrar_estagiario TO supervisor;
go

-- PROCEDURE visualizar_estagiario_especifico

CREATE PROCEDURE rh.visualizar_estagiario_especifico @nome varchar(80) AS
SELECT p.nome as polo, 
	   e.instituicao as instituicao, e.curso as curso, 
	   CONVERT(varchar(10), e.inicio_curso, 103) as inicio_curso,
	   CONVERT(varchar(10), e.fim_curso, 103) as fim_curso,
	   CONVERT(varchar(10), e.data_contrato, 103) as data_contrato,
  	   CONVERT(varchar(5), e.horario_entrada, 108) as horario_entrada,
	   CONVERT(varchar(5), e.horario_saida, 108) as horario_saida
	   FROM rh.funcionario f INNER JOIN rh.estagiario e
	   ON f.id = e.id INNER JOIN vc.polo p
	   ON e.polo = p.id
	   WHERE f.id = (SELECT id FROM rh.funcionario WHERE nome = @nome)
GO

GRANT EXECUTE ON rh.visualizar_estagiario_especifico TO supervisor;
go

-- PROCEDURE visualizar_polo_especifico

CREATE PROCEDURE vc.visualizar_polo_especifico @nome varchar(80) AS
SELECT p.nome, p.endereco, p.cidade, p.capacidade,
	   CONVERT(varchar(5), p.horario_abertura, 108) as horario_abertura, 
	   CONVERT(varchar(5), p.horario_fechamento, 108) as horario_fechamento,
	   p.email, p.senha, p.celular
	   FROM vc.polo p
	   WHERE p.nome = @nome;
go

GRANT EXECUTE ON vc.visualizar_polo_especifico TO supervisor
go

-- VIEW visualizar_polo

CREATE VIEW vc.visualizar_polo AS
SELECT p.nome, p.endereco, p.cidade, p.capacidade,
	   CONVERT(varchar(5), p.horario_abertura, 108) as horario_abertura, 
	   CONVERT(varchar(5), p.horario_fechamento, 108) as horario_fechamento,
	   p.email, p.senha, p.celular, 

	   (SELECT TOP 1 (f.nome) FROM rh.funcionario f INNER JOIN rh.estagiario e
		ON f.id = e.id WHERE e.polo = (SELECT id FROM vc.polo WHERE id = p.id)
		ORDER BY e.horario_entrada ASC) AS estagiario_manha, 

	   (SELECT TOP 1 (f.nome) AS estagiario FROM rh.funcionario f INNER JOIN rh.estagiario e
		ON f.id = e.id WHERE e.polo = (SELECT id FROM vc.polo WHERE id = p.id)
		ORDER BY e.horario_entrada DESC) AS estagiario_tarde

	   FROM vc.polo p
go

GRANT SELECT ON vc.visualizar_polo TO supervisor;
go

-- FUNCTION contagem_titulo
CREATE FUNCTION vc.contagem_titulo (@username varchar(16))
RETURNS @tabela TABLE (codigo int, titulo varchar(80)) AS
BEGIN
DECLARE @codigo int;
DECLARE @titulo varchar(80);
SELECT @codigo = v.codigo, @titulo = v.titulo
	FROM vc.videoconferencia v INNER JOIN vc.ativado a 
	ON v.codigo = a.video INNER JOIN vc.polo p 
	ON a.polo = p.id INNER JOIN rh.estagiario e
	ON p.id = e.polo
	WHERE e.id = (SELECT f.id FROM rh.funcionario f INNER JOIN rh.login l ON f.id = l.id WHERE l.username = @username)
	AND (data_ativacao = CONVERT (varchar(10), GETDATE(), 103)
	AND CONVERT (varchar(5), GETDATE(), 108) BETWEEN v.horario_inicio AND v.horario_fim)

INSERT INTO @tabela (codigo, titulo) VALUES (@codigo, @titulo);
RETURN;
END;
go

GRANT SELECT ON vc.contagem_titulo TO estagiario;
GO

-- PROCEDURE contagem_insert
CREATE PROCEDURE vc.contagem_insert @username varchar(16), @contagem int AS
UPDATE vc.ativado SET contagem = @contagem 
	WHERE video = (SELECT codigo FROM vc.contagem_titulo (@username)) 
	AND polo = (SELECT e.polo FROM rh.funcionario f INNER JOIN rh.estagiario e 
				ON f.id = e.id INNER JOIN rh.login l ON f.id = l.id 
				WHERE l.username = @username);

UPDATE vc.ativado SET horario_contagem = GETDATE()
	WHERE video = (SELECT codigo FROM vc.contagem_titulo (@username)) 
	AND polo = (SELECT e.polo FROM rh.funcionario f INNER JOIN rh.estagiario e 
				ON f.id = e.id INNER JOIN rh.login l ON f.id = l.id 
				WHERE l.username = @username);
				
UPDATE vc.ativado SET estagiario = (SELECT e.id FROM rh.funcionario f INNER JOIN rh.estagiario e 
									ON f.id = e.id INNER JOIN rh.login l ON f.id = l.id 
									WHERE l.username = @username)
	WHERE video = (SELECT codigo FROM vc.contagem_titulo (@username)) 
	AND polo = (SELECT e.polo FROM rh.funcionario f INNER JOIN rh.estagiario e 
				ON f.id = e.id INNER JOIN rh.login l ON f.id = l.id 
				WHERE l.username = @username);				
GO

GRANT EXECUTE ON vc.contagem_insert TO estagiario;
GO


-- FUNCTION contagem_check

CREATE FUNCTION vc.contagem_check (@username varchar(16))
RETURNS INT AS
BEGIN

DECLARE @horario_contagem datetime;

SELECT @horario_contagem = horario_contagem FROM vc.ativado 
	WHERE video = (SELECT codigo FROM vc.contagem_titulo (@username)) 
	AND polo = (SELECT e.polo FROM rh.funcionario f INNER JOIN rh.estagiario e 
				ON f.id = e.id INNER JOIN rh.login l ON f.id = l.id 
				WHERE l.username = @username);

IF @horario_contagem IS NOT NULL
RETURN 1
ELSE
RETURN 0

RETURN NULL;
END;
GO

GRANT EXECUTE ON vc.contagem_check TO estagiario;
GO

-- VIEW visualizar_videoconferencia

CREATE VIEW vc.visualizar_videoconferencia AS
SELECT codigo, titulo, 
CONVERT(char(10), data_ativacao, 103) as data_ativacao, 
CONVERT(char(5), horario_inicio, 108) as horario_inicio,
CONVERT(char(5), horario_fim, 108) as horario_fim
FROM vc.videoconferencia;
go

GRANT SELECT ON vc.visualizar_videoconferencia TO supervisor
go

-- PROCEDURE visualizar_contagem

CREATE PROCEDURE vc.visualizar_contagem @string varchar (100) AS
DECLARE @id int;
SELECT @id = SUBSTRING(@string, 
					   CHARINDEX('[', @string) + 1,
					   CHARINDEX(']', @string) - CHARINDEX('[', @string) - 1);


SELECT p.nome as polo, a.contagem, 
CONVERT(char(10), a.horario_contagem, 103) + ' ' + CONVERT(char(5), a.horario_contagem, 108) as horario_contagem
FROM vc.ativado a INNER JOIN vc.polo p
	 ON p.id = a.polo INNER JOIN vc.videoconferencia v
	 ON a.video = v.codigo
	 WHERE a.video = @id
go

GRANT EXECUTE ON vc.visualizar_contagem TO supervisor
go

-- FUNCTION apontamento_verificador

CREATE FUNCTION rh.apontamento_verificador (@username varchar(16))
RETURNS integer AS
BEGIN

IF EXISTS
(
	SELECT a.funcionario, a.apontamento_entrada FROM rh.apontamento a INNER JOIN rh.funcionario f
	ON a.funcionario = (SELECT f.id FROM rh.funcionario f INNER JOIN rh.login l ON f.id = l.id WHERE l.username = @username)
	WHERE CONVERT(date, apontamento_entrada) = CONVERT(date, CURRENT_TIMESTAMP)
)
BEGIN
	IF EXISTS
	(
		SELECT a.funcionario, a.apontamento_saida FROM rh.apontamento a INNER JOIN rh.funcionario f
		ON a.funcionario = (SELECT f.id FROM rh.funcionario f INNER JOIN rh.login l ON f.id = l.id WHERE l.username = @username)
		WHERE CONVERT(date, apontamento_saida) = CONVERT(date, CURRENT_TIMESTAMP)
	)
	BEGIN
		RETURN 2;
	END;
	ELSE
	BEGIN
		RETURN 1;
	END;
END;

ELSE
BEGIN
	RETURN 0;
END;

RETURN NULL
END;
go

GRANT EXECUTE ON rh.apontamento_verificador TO estagiario;
go

-- PROCEDURE apontamento_entrada

CREATE PROCEDURE rh.apontamento_entrada @username varchar(16), @justificativa varchar(255) AS
INSERT INTO rh.apontamento (funcionario, polo, apontamento_entrada, justificativa) VALUES 
	((SELECT f.id FROM rh.funcionario f INNER JOIN rh.login l ON f.id = l.id WHERE l.username = @username), 
	 (SELECT e.polo FROM rh.estagiario e INNER JOIN rh.login l ON e.id = l.id WHERE l.username = @username),
	 CURRENT_TIMESTAMP, @justificativa);
GO

GRANT EXECUTE ON rh.apontamento_entrada TO estagiario;
go

-- PROCEDURE apontamento_saida
CREATE PROCEDURE rh.apontamento_saida @username varchar(16), @justificativa varchar(255) AS
UPDATE rh.apontamento SET apontamento_saida = CURRENT_TIMESTAMP 
		WHERE funcionario = (SELECT f.id FROM rh.funcionario f INNER JOIN rh.login l ON f.id = l.id WHERE l.username = @username)
		AND CONVERT(date, apontamento_entrada) = CONVERT(date, CURRENT_TIMESTAMP);

DECLARE @justificativa_atual varchar (127);
SELECT @justificativa_atual = justificativa FROM rh.apontamento
WHERE funcionario = (SELECT f.id FROM rh.funcionario f INNER JOIN rh.login l ON f.id = l.id WHERE l.username = @username)
		AND CONVERT(date, apontamento_entrada) = CONVERT(date, CURRENT_TIMESTAMP);

SET @justificativa_atual += @justificativa;
UPDATE rh.apontamento SET justificativa = @justificativa_atual
		WHERE funcionario = (SELECT f.id FROM rh.funcionario f INNER JOIN rh.login l ON f.id = l.id WHERE l.username = @username)
		AND CONVERT(date, apontamento_entrada) = CONVERT(date, CURRENT_TIMESTAMP);
GO

GRANT EXECUTE ON rh.apontamento_saida TO estagiario;
go

-- VIEW apontamento_visualizar

CREATE VIEW rh.apontamento_visualizar AS
SELECT f.nome, CONVERT(char(5), a.apontamento_entrada, 108) as apontamento_entrada, 
CONVERT(char(5), a.apontamento_saida, 108) as apontamento_saida, 
CONVERT(char(10), a.apontamento_entrada, 103) as dia, a.justificativa
FROM rh.apontamento a INNER JOIN rh.funcionario f 
ON a.funcionario = f.id;
go

GRANT SELECT ON rh.apontamento_visualizar TO supervisor;
go

-- PROCEDURE estagiario_dados

CREATE PROCEDURE rh.estagiario_dados @username varchar(16) AS
SELECT f.nome, f.cpf, f.rg, f.sexo, t.telefone, f.data_nasc, 
f.logradouro, f.numero, f.complemento, f.cep, f.cidade,
e.instituicao, e.curso, e.inicio_curso, e.fim_curso, f.email 
FROM rh.funcionario f INNER JOIN rh.estagiario e
ON f.id = e.id INNER JOIN rh.telefone t
ON f.id = t.id
WHERE f.id = (SELECT f.id FROM rh.funcionario f INNER JOIN rh.login l ON f.id = l.id WHERE l.username = @username);
go

GRANT EXECUTE ON rh.estagiario_dados TO estagiario;
go

-- PROCEDURE polo_email

CREATE PROCEDURE vc.polo_email @username varchar(16) AS	
SELECT p.email FROM vc.polo p INNER JOIN rh.estagiario e
ON p.id = e.polo WHERE 
e.id = (SELECT e.id FROM rh.funcionario f INNER JOIN rh.estagiario e 
		ON f.id = e.id INNER JOIN rh.login l ON f.id = l.id 
		WHERE l.username = @username);
go

GRANT EXECUTE ON vc.polo_email TO estagiario;
go