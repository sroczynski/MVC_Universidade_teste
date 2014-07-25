CREATE DATABASE TESTE;
-- Exemplo: 1 | Nicolas Vinícius Sroczynski | nicolas.sroczynski@gmail.com | 17/10/1995 | 1 
-- Exemplo: 2 | TESTE                       | teste@gmail.com              | 17/10/1995 | 2 

CREATE TABLE Aluno(
	alunoID INTEGER IDENTITY(1,1) PRIMARY KEY NOT NULL,
	nome	VARCHAR(50) NOT NULL,
	email	VARCHAR(50),
	dataNascimento DATE,
	cursoID INTEGER,
	CONSTRAINT FK_aluno_curso FOREIGN KEY(cursoID) REFERENCES curso(cursoID)
 );

 -- Exemplo: 1 | Universidade Feevale | Novo Hamburgo
 -- Exemplo: 1 | Unisinos             | São Leopoldo
CREATE TABLE Universidade(
	universidadeID INTEGER IDENTITY(1,1) PRIMARY KEY NOT NULL,
	nome	VARCHAR(50) NOT NULL,
	cidade	VARCHAR(50) NOT NULL
);

-- Exemplo: 1 | Sistemas para internet | 1 
-- Exemplo: 2 | Ciências da computação | 1
-- Exemplo: 3 | Sistemas de informação | 1
CREATE TABLE Curso( 
	cursoID INTEGER IDENTITY(1,1) PRIMARY KEY NOT NULL,
	nome	VARCHAR(50) NOT NULL,
	universidadeID INTEGER NOT NULL,
	CONSTRAINT FK_curso_universidade FOREIGN KEY(universidadeID) REFERENCES universidade(universidadeID)
);

 -- Exemplo: 1 | 1 | Banco de dados |
 -- Exemplo: 2 | 1 | Padrões p/ web | 
 CREATE TABLE Disciplina( 
	disciplinaID INTEGER IDENTITY(1,1) PRIMARY KEY NOT NULL,
	cursoID INTEGER,
	nome	VARCHAR(50) NOT NULL,
	CONSTRAINT FK_disciplina_curso FOREIGN KEY(cursoID) REFERENCES curso(cursoID)
);

-- Exemplo: 1 | 1 | 10
-- Exemplo: 1 | 2 | 09 
CREATE TABLE Notas(  
	alunoID INTEGER NOT NULL,
	disciplinaID INTEGER NOT NULL,
	nota INTEGER NOT NULL,	
	CONSTRAINT PK_notas PRIMARY KEY(alunoID,disciplinaID),
	CONSTRAINT FK_notas_aluno FOREIGN KEY(alunoID) REFERENCES aluno(alunoID),
	CONSTRAINT FK_notas_disciplina FOREIGN KEY(disciplinaId) REFERENCES disciplina(disciplinaID)
);  

-- Inserts
--aluno
INSERT INTO Aluno(nome, email,dataNascimento, cursoID) VALUES ('Nícolas Vinícius Sroczynski', 'nicolas.sroczynski@gmail.com', '1995-10-17',1);
INSERT INTO aluno(nome, email,dataNascimento, cursoID) VALUES ('teste', 'teste@gmail.com', GETDATE(), 1)
SELECT * FROM ALUNO
--Universidade
INSERT INTO universidade(nome, cidade) VALUES ('Universidade Feevale', 'Novo Hamburgo');
INSERT INTO universidade(nome, cidade) VALUES ('Unisinos', 'São Leopoldo');
SELECT * FROM Universidade
--Curso
INSERT INTO curso(nome, universidadeID) VALUES ('Sistemas para Internet', 1);
INSERT INTO curso(nome, universidadeID) VALUES ('Ciência da computação', 1);
INSERT INTO curso(nome, universidadeID) VALUES ('Sistemas de informação', 2);
SELECT * FROM Curso
--Disciplina
INSERT INTO disciplina(cursoID, nome) VALUES (1, 'Padrões Web');
INSERT INTO disciplina(cursoID, nome) VALUES (1, 'Programação para internet');
INSERT INTO disciplina(cursoID, nome) VALUES (2, 'Cálculo 1');
INSERT INTO disciplina(cursoID, nome) VALUES (2, 'Cálculo 2');
INSERT INTO disciplina(cursoID, nome) VALUES (2, 'Cálculo 3');
INSERT INTO disciplina(cursoID, nome) VALUES (4, 'Teste');

select * from Disciplina

--Notas Exemplo: 1 | 2 | 09 


INSERT INTO notas(alunoID, disciplinaID, nota) VALUES (2, 3, 9);
INSERT INTO notas(alunoID, disciplinaID, nota) VALUES (2, 4, 8);
INSERT INTO notas(alunoID, disciplinaID, nota) VALUES (2, 5, 10);

INSERT INTO notas(alunoID,disciplinaID, nota) VALUES (3, 1 , 10);
INSERT INTO notas(alunoID, disciplinaID, nota) VALUES (3, 2, 9);
INSERT INTO notas(alunoID, disciplinaID, nota) VALUES (3, 3, 8);



SELECT * FROM Aluno
SELECT * FROM Notas
SELECT * FROM Curso

SELECT * FROM Universidade

delete Aluno from Aluno where alunoID = 2



-- Todas as notas dos cursos do aluno 1
select al.nome as Aluno, di.nome as Disciplina, no.nota
from notas no inner join aluno al 
on no.alunoID = al.alunoID
inner join disciplina di 
on no.disciplinaID = di.disciplinaID
where al.alunoID = 1

select al.nome, di.nome, no.nota
from notas no inner join aluno al 
on no.alunoID = al.alunoID
inner join disciplina di 
on no.disciplinaID = di.disciplinaID
where al.alunoID = 2


select * from notas
-- Query utilizada na aplicação para retornar todas as notas, porém não na view não exibe o primeiro registro
select al.nome as NomeAluno, di.nome as NomeDisciplina, no.nota as Nota from notas no inner join aluno al on no.alunoID = al.alunoID inner join disciplina di on no.disciplinaID = di.disciplinaID


select alunoID, nome, email, dataNascimento, cursoID from aluno
select alunoID, nome, email, dataNascimento, cursoID from aluno where alunoID = 1

select * from universidade;


select al.alunoID as alunoID, al.nome as alunoNome, al.email as alunoEmail, al.dataNascimento as dataNascimento, c.cursoID as cursoID, c.nome as cursoNome, un.universidadeID as universidadeID, un.nome as universidadeNome from aluno al inner join curso c on al.cursoID = c.cursoID inner join universidade un on c.universidadeID = un.universidadeID order by al.alunoID

select c.cursoID as cursoID, c.nome as cursoNome, un.universidadeID as universidadeID, un.nome as universidadeNome from curso c inner join universidade un on c.universidadeID = un.universidadeID order by c.cursoID

--select do model de discipçinas
select di.disciplinaID as disciplinaID, di.nome as nomeDisciplina, cur.cursoID as cursoID, cur.nome as nomeCurso, un.universidadeID as universidadeID, un.nome as nomeUniversidade
from Disciplina di 
inner join curso cur 
on di.cursoID = cur.cursoID
inner join universidade un 
on cur.universidadeID = un.universidadeID


select al.nome, dis.nome, n.nota
from aluno al inner join curso cur 
on al.cursoID = cur.cursoID 
inner join disciplina dis 
on cur.cursoID = dis.cursoID
inner join Notas n 
on dis.disciplinaID = n.disciplinaID


select * from Notas


select * from Disciplina