




-- -----------------------------------------------------------
-- Entity Designer DDL Script for MySQL Server 4.1 and higher
-- -----------------------------------------------------------
-- Date Created: 11/09/2015 11:16:29
-- Generated from EDMX file: C:\Users\Mark Haurum\Documents\UNI\3. Semester\P3\CupPlaner\CupPlaner\CupDB.edmx
-- Target version: 3.0.0.0
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- NOTE: if the constraint does not exist, an ignorable error will be reported.
-- --------------------------------------------------

--    ALTER TABLE `TeamMatch` DROP CONSTRAINT `FK_TeamMatch_Team`;
--    ALTER TABLE `TeamMatch` DROP CONSTRAINT `FK_TeamMatch_Match`;
--    ALTER TABLE `TeamSet` DROP CONSTRAINT `FK_PoolTeam`;
--    ALTER TABLE `TimeIntervalSet` DROP CONSTRAINT `FK_TeamTimeInterval`;
--    ALTER TABLE `PoolSet` DROP CONSTRAINT `FK_DivisionPool`;
--    ALTER TABLE `DivisionTournamentSet` DROP CONSTRAINT `FK_DivisionTournamentDivision`;
--    ALTER TABLE `DivisionSet` DROP CONSTRAINT `FK_TournamentDivision`;
--    ALTER TABLE `TournamentStageSet` DROP CONSTRAINT `FK_DivisionTournamentTournamentStage`;
--    ALTER TABLE `MatchSet` DROP CONSTRAINT `FK_MatchField`;
--    ALTER TABLE `PoolField` DROP CONSTRAINT `FK_PoolField_Pool`;
--    ALTER TABLE `PoolField` DROP CONSTRAINT `FK_PoolField_Field`;
--    ALTER TABLE `TimeIntervalSet` DROP CONSTRAINT `FK_TournamentTimeInterval`;
--    ALTER TABLE `DivisionTournamentSet` DROP CONSTRAINT `FK_TournamentDivisionTournament`;
--    ALTER TABLE `MatchSet` DROP CONSTRAINT `FK_TournamentStageMatch`;
--    ALTER TABLE `TournamentStageSet` DROP CONSTRAINT `FK_TournamentStagePool`;

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------
SET foreign_key_checks = 0;
    DROP TABLE IF EXISTS `TeamSet`;
    DROP TABLE IF EXISTS `PoolSet`;
    DROP TABLE IF EXISTS `DivisionSet`;
    DROP TABLE IF EXISTS `DivisionTournamentSet`;
    DROP TABLE IF EXISTS `FieldSet`;
    DROP TABLE IF EXISTS `TimeIntervalSet`;
    DROP TABLE IF EXISTS `TournamentSet`;
    DROP TABLE IF EXISTS `MatchSet`;
    DROP TABLE IF EXISTS `TournamentStageSet`;
    DROP TABLE IF EXISTS `TeamMatch`;
    DROP TABLE IF EXISTS `PoolField`;
SET foreign_key_checks = 1;

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

CREATE TABLE `TeamSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Name` longtext NOT NULL, 
	`Pool_Id` int NOT NULL);

ALTER TABLE `TeamSet` ADD PRIMARY KEY (Id);




CREATE TABLE `PoolSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Name` longtext NOT NULL, 
	`Division_Id` int NOT NULL);

ALTER TABLE `PoolSet` ADD PRIMARY KEY (Id);




CREATE TABLE `DivisionSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Name` longtext NOT NULL, 
	`FieldSize` int NOT NULL, 
	`MatchDuration` int NOT NULL, 
	`Tournament_Id` int NOT NULL);

ALTER TABLE `DivisionSet` ADD PRIMARY KEY (Id);




CREATE TABLE `DivisionTournamentSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`TournamentStructure` int NOT NULL, 
	`Division_Id` int NOT NULL, 
	`Tournament_Id` int NOT NULL);

ALTER TABLE `DivisionTournamentSet` ADD PRIMARY KEY (Id);




CREATE TABLE `FieldSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Name` longtext NOT NULL, 
	`Size` int NOT NULL);

ALTER TABLE `FieldSet` ADD PRIMARY KEY (Id);




CREATE TABLE `TimeIntervalSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`StartTime` datetime( 3 )  NOT NULL, 
	`EndTime` datetime( 3 )  NOT NULL, 
	`Team_Id` int, 
	`Tournament_Id` int);

ALTER TABLE `TimeIntervalSet` ADD PRIMARY KEY (Id);




CREATE TABLE `TournamentSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Name` longtext NOT NULL, 
	`Password` longtext NOT NULL);

ALTER TABLE `TournamentSet` ADD PRIMARY KEY (Id);




CREATE TABLE `MatchSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`StartTime` datetime( 3 )  NOT NULL, 
	`Duration` int NOT NULL, 
	`Field_Id` int, 
	`TournamentStage_Id` int NOT NULL);

ALTER TABLE `MatchSet` ADD PRIMARY KEY (Id);




CREATE TABLE `TournamentStageSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`TournamentStructure` int NOT NULL, 
	`DivisionTournament_Id` int NOT NULL, 
	`Pool_Id` int NOT NULL);

ALTER TABLE `TournamentStageSet` ADD PRIMARY KEY (Id);




CREATE TABLE `TeamMatch`(
	`Teams_Id` int NOT NULL, 
	`Matches_Id` int NOT NULL);

ALTER TABLE `TeamMatch` ADD PRIMARY KEY (Teams_Id, Matches_Id);




CREATE TABLE `PoolField`(
	`Pool_Id` int NOT NULL, 
	`FavoriteFields_Id` int NOT NULL);

ALTER TABLE `PoolField` ADD PRIMARY KEY (Pool_Id, FavoriteFields_Id);






-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on `Teams_Id` in table 'TeamMatch'

ALTER TABLE `TeamMatch`
ADD CONSTRAINT `FK_TeamMatch_Team`
    FOREIGN KEY (`Teams_Id`)
    REFERENCES `TeamSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating foreign key on `Matches_Id` in table 'TeamMatch'

ALTER TABLE `TeamMatch`
ADD CONSTRAINT `FK_TeamMatch_Match`
    FOREIGN KEY (`Matches_Id`)
    REFERENCES `MatchSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TeamMatch_Match'

CREATE INDEX `IX_FK_TeamMatch_Match` 
    ON `TeamMatch`
    (`Matches_Id`);

-- Creating foreign key on `Pool_Id` in table 'TeamSet'

ALTER TABLE `TeamSet`
ADD CONSTRAINT `FK_PoolTeam`
    FOREIGN KEY (`Pool_Id`)
    REFERENCES `PoolSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PoolTeam'

CREATE INDEX `IX_FK_PoolTeam` 
    ON `TeamSet`
    (`Pool_Id`);

-- Creating foreign key on `Team_Id` in table 'TimeIntervalSet'

ALTER TABLE `TimeIntervalSet`
ADD CONSTRAINT `FK_TeamTimeInterval`
    FOREIGN KEY (`Team_Id`)
    REFERENCES `TeamSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TeamTimeInterval'

CREATE INDEX `IX_FK_TeamTimeInterval` 
    ON `TimeIntervalSet`
    (`Team_Id`);

-- Creating foreign key on `Division_Id` in table 'PoolSet'

ALTER TABLE `PoolSet`
ADD CONSTRAINT `FK_DivisionPool`
    FOREIGN KEY (`Division_Id`)
    REFERENCES `DivisionSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DivisionPool'

CREATE INDEX `IX_FK_DivisionPool` 
    ON `PoolSet`
    (`Division_Id`);

-- Creating foreign key on `Division_Id` in table 'DivisionTournamentSet'

ALTER TABLE `DivisionTournamentSet`
ADD CONSTRAINT `FK_DivisionTournamentDivision`
    FOREIGN KEY (`Division_Id`)
    REFERENCES `DivisionSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DivisionTournamentDivision'

CREATE INDEX `IX_FK_DivisionTournamentDivision` 
    ON `DivisionTournamentSet`
    (`Division_Id`);

-- Creating foreign key on `Tournament_Id` in table 'DivisionSet'

ALTER TABLE `DivisionSet`
ADD CONSTRAINT `FK_TournamentDivision`
    FOREIGN KEY (`Tournament_Id`)
    REFERENCES `TournamentSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TournamentDivision'

CREATE INDEX `IX_FK_TournamentDivision` 
    ON `DivisionSet`
    (`Tournament_Id`);

-- Creating foreign key on `DivisionTournament_Id` in table 'TournamentStageSet'

ALTER TABLE `TournamentStageSet`
ADD CONSTRAINT `FK_DivisionTournamentTournamentStage`
    FOREIGN KEY (`DivisionTournament_Id`)
    REFERENCES `DivisionTournamentSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DivisionTournamentTournamentStage'

CREATE INDEX `IX_FK_DivisionTournamentTournamentStage` 
    ON `TournamentStageSet`
    (`DivisionTournament_Id`);

-- Creating foreign key on `Field_Id` in table 'MatchSet'

ALTER TABLE `MatchSet`
ADD CONSTRAINT `FK_MatchField`
    FOREIGN KEY (`Field_Id`)
    REFERENCES `FieldSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MatchField'

CREATE INDEX `IX_FK_MatchField` 
    ON `MatchSet`
    (`Field_Id`);

-- Creating foreign key on `Pool_Id` in table 'PoolField'

ALTER TABLE `PoolField`
ADD CONSTRAINT `FK_PoolField_Pool`
    FOREIGN KEY (`Pool_Id`)
    REFERENCES `PoolSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating foreign key on `FavoriteFields_Id` in table 'PoolField'

ALTER TABLE `PoolField`
ADD CONSTRAINT `FK_PoolField_Field`
    FOREIGN KEY (`FavoriteFields_Id`)
    REFERENCES `FieldSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PoolField_Field'

CREATE INDEX `IX_FK_PoolField_Field` 
    ON `PoolField`
    (`FavoriteFields_Id`);

-- Creating foreign key on `Tournament_Id` in table 'TimeIntervalSet'

ALTER TABLE `TimeIntervalSet`
ADD CONSTRAINT `FK_TournamentTimeInterval`
    FOREIGN KEY (`Tournament_Id`)
    REFERENCES `TournamentSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TournamentTimeInterval'

CREATE INDEX `IX_FK_TournamentTimeInterval` 
    ON `TimeIntervalSet`
    (`Tournament_Id`);

-- Creating foreign key on `Tournament_Id` in table 'DivisionTournamentSet'

ALTER TABLE `DivisionTournamentSet`
ADD CONSTRAINT `FK_TournamentDivisionTournament`
    FOREIGN KEY (`Tournament_Id`)
    REFERENCES `TournamentSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TournamentDivisionTournament'

CREATE INDEX `IX_FK_TournamentDivisionTournament` 
    ON `DivisionTournamentSet`
    (`Tournament_Id`);

-- Creating foreign key on `TournamentStage_Id` in table 'MatchSet'

ALTER TABLE `MatchSet`
ADD CONSTRAINT `FK_TournamentStageMatch`
    FOREIGN KEY (`TournamentStage_Id`)
    REFERENCES `TournamentStageSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TournamentStageMatch'

CREATE INDEX `IX_FK_TournamentStageMatch` 
    ON `MatchSet`
    (`TournamentStage_Id`);

-- Creating foreign key on `Pool_Id` in table 'TournamentStageSet'

ALTER TABLE `TournamentStageSet`
ADD CONSTRAINT `FK_TournamentStagePool`
    FOREIGN KEY (`Pool_Id`)
    REFERENCES `PoolSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TournamentStagePool'

CREATE INDEX `IX_FK_TournamentStagePool` 
    ON `TournamentStageSet`
    (`Pool_Id`);

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
