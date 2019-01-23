Para o back-end foi escolhido o ASP.NET MVC framework 4.5.2. Foi utilizado o padrão DAL para abstração da camada de dados,
e para o acesso e manipulação foi utilizado o Entity FrameWork 6.
No front-end, foi escolhido o Bootstrap e JQuery.  Utilizei FormsAutentication para o login do usuário. A senhas dos usuários são salvas no banco 
utilizando para a e criptografia a função PBKDF2. O campo Salt adicionado na tabela UserSys serve para fazer a
a autenticação do usuário no momento do login.
