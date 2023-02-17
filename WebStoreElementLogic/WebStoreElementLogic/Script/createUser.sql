USE [master]
GO

/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [admin]    Script Date: 12.02.2023 19:55:16 ******/
CREATE LOGIN [admin] WITH PASSWORD=N'JNI+b4ISHVuyAhbqns+ewlDx+5GV9cZaVbR4PtYGsaQ=', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

ALTER LOGIN [admin] DISABLE
GO

ALTER SERVER ROLE [sysadmin] ADD MEMBER [admin]
GO

