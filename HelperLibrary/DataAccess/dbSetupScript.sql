create table Users
(
    Id             int auto_increment
        primary key,
    Username       varchar(50)                 not null,
    HashedPassword varchar(70)                 not null,
    Salt           varchar(70)                 not null,
    Email          varchar(80)                 not null,
    Balance        decimal(15, 2) default 0.00 not null,
    constraint Email
        unique (Email),
    constraint Username
        unique (Username)
);

create user if not exists 'BJUser'@'localhost' identified by 'BJUserPassword123';

create procedure spAddMoney(IN uname varchar(50), IN amount decimal(15, 2))
begin 
    update Users set Balance = Balance + amount where Username = uname;
end;

grant execute on procedure spAddMoney to BJUser@localhost;

create procedure spAddUser(IN uname varchar(50), IN pass varchar(70), IN slt varchar(70), IN em varchar(80))
begin 
    insert into Users(Username, HashedPassword, Users.Salt, Users.Email)
        values (uname, pass, slt, em);
end;

grant execute on procedure spAddUser to BJUser@localhost;

create procedure spChangePassword(IN uname varchar(50), IN newPass varchar(70), IN newSalt varchar(70))
begin 
    update Users set HashedPassword = newPass, Users.Salt = newSalt where Username = uname;
end;

grant execute on procedure spChangePassword to BJUser@localhost;

create procedure spCheckUserExists(IN uname varchar(50), IN pass varchar(70))
begin
    select count(*) from Users where Username = uname and HashedPassword = pass;
end;

grant execute on procedure spCheckUserExists to BJUser@localhost;

create procedure spCheckUserRepeats(IN uname varchar(50), IN em varchar(80))
begin 
    select count(*) from Users where Username = uname or Users.Email = em;
end;

grant execute on procedure spCheckUserRepeats to BJUser@localhost;

create procedure spGetSalt(IN uname varchar(50))
begin 
    select Users.Salt from Users where Username = uname;
end;

grant execute on procedure spGetSalt to BJUser@localhost;

create procedure spGetUserBalance(IN uname varchar(50))
begin
    select Balance from Users where Username = uname;
end;

grant execute on procedure spGetUserBalance to BJUser@localhost;

create procedure spRemoveMoney(IN amount decimal(15, 2), IN uname varchar(50))
begin
    update Users set Balance = Balance - amount where Username = uname;
end;

grant execute on procedure spRemoveMoney to BJUser@localhost;

create procedure spRemoveUser(IN uname varchar(50))
begin 
    delete from Users where Username = uname;
end;

grant execute on procedure spRemoveUser to BJUser@localhost;


