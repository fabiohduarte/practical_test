Para o back-end foi escolhido o ASP.NET MVC framework 4.5.2. Foi utilizado o padr�o DAL para abstra��o da camada de dados,
e para o acesso e manipula��o foi utilizado o Entity FrameWork 6.
No front-end, foi escolhido o Bootstrap e JQuery.  Utilizei FormsAutentication para o login do usu�rio. A senhas dos usu�rios s�o salvas no banco 
utilizando para a e criptografia a fun��o PBKDF2. O campo Salt adicionado na tabela UserSys serve para fazer a
a autentica��o do usu�rio no momento do login.
