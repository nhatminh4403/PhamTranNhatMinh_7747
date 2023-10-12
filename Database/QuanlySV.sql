create database QuanlySV
go

use QuanlySV
go

create table Lop
(
	MaLop char(3) primary key not null,
	TenLop nvarchar(30) not null
)

create table Sinhvien
(
	MaSV char(6) primary key not null,
	HotenSV nvarchar(40) not null,
	MaLop char(3) not null,
	foreign key(MaLop) references Lop(MaLop)
)

insert into Lop 
values ('IT1', N'Công nghệ thông tin'),
	   ('KD1',N'Quản trị kinh doanh')

insert into Sinhvien
values	('K27747',N'Phạm Hoàng Minh','IT1'),
		('K14567',N'Trần Thiên Ngân','KD1'),
		('K27847',N'Hoàng Ngọc Long','IT1'),
		('K14576',N'Nguyễn Phù Vân Thiên','KD1')