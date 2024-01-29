use InvestingIS;

CREATE TABLE Companies (
	company_id INT IDENTITY(1, 1) NOT NULL,
	PRIMARY KEY CLUSTERED (company_id ASC),
	company_name VARCHAR(48) NOT NULL
);

CREATE TABLE Users (
	user_id INT IDENTITY(1, 1) NOT NULL,
	PRIMARY KEY CLUSTERED (user_id ASC),
	-- user_id INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	user_surname VARCHAR(24) NOT NULL,
	user_name VARCHAR(24) NOT NULL,
	user_patronymic VARCHAR(24) DEFAULT NULL,
	user_login VARCHAR(24) NOT NULL,
	user_password VARCHAR(20) NOT NULL,
	user_role VARCHAR(24) DEFAULT '€нвестор' NOT NULL
);

-- ценные бумаги
CREATE TABLE SecurityPapers (
	security_id INT IDENTITY(1, 1) NOT NULL,
	PRIMARY KEY CLUSTERED (security_id ASC),
	security_name VARCHAR(24) NOT NULL,
	security_open_cost MONEY NOT NULL,
	security_close_cost MONEY NOT NULL,
	security_cost MONEY NOT NULL,
	company_id INT NOT NULL,
	FOREIGN KEY (company_id) REFERENCES Companies(company_id) ON DELETE CASCADE
);

-- портфели
CREATE TABLE Bags (
	bag_id INT IDENTITY(1, 1) NOT NULL,
	PRIMARY KEY CLUSTERED (bag_id ASC),
	bag_name VARCHAR(24) NOT NULL,
	bag_balance MONEY NOT NULL,
	bag_transactions_total_cost MONEY NOT NULL,
	user_id INT NOT NULL,
	FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

CREATE TABLE Transactions (
	transaction_id INT IDENTITY(1, 1) NOT NULL,
	PRIMARY KEY CLUSTERED (transaction_id ASC),
	bag_id INT NOT NULL,
	FOREIGN KEY (bag_id) REFERENCES Bags(bag_id) ON DELETE CASCADE,
	transaction_date DATETIME NOT NULL,
	transaction_type VARCHAR(24) NOT NULL,
	security_id INT NOT NULL,
	FOREIGN KEY (security_id) REFERENCES SecurityPapers(security_id) ON DELETE CASCADE,
	security_papers_count INT NOT NULL,
	transaction_cost MONEY NOT NULL
);

CREATE TABLE FinancialReports (
	financial_report_date DATETIME NOT NULL,
	financial_description VARCHAR(512) NOT NULL
);

DROP TABLE Companies
DROP TABLE Users
DROP TABLE SecurityPapers
DROP TABLE Bags
DROP TABLE Transactions
DROP TABLE FinancialReports

INSERT INTO Users ([user_surname], [user_name], [user_patronymic], [user_login], [user_password], [user_role]) VALUES ('Брокеров', 'Брокер', 'Брокерович', 'broker',  '__broker00', 'Брокер')