




-- -----------------------------------------------------------
-- Entity Designer DDL Script for MySQL Server 4.1 and higher
-- -----------------------------------------------------------
-- Date Created: 11/24/2015 11:10:36
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
--    ALTER TABLE `DivisionSet` DROP CONSTRAINT `FK_TournamentDivision`;
--    ALTER TABLE `MatchSet` DROP CONSTRAINT `FK_MatchField`;
--    ALTER TABLE `PoolField` DROP CONSTRAINT `FK_PoolField_Pool`;
--    ALTER TABLE `PoolField` DROP CONSTRAINT `FK_PoolField_Field`;
--    ALTER TABLE `TimeIntervalSet` DROP CONSTRAINT `FK_TournamentTimeInterval`;
--    ALTER TABLE `MatchSet` DROP CONSTRAINT `FK_TournamentStageMatch`;
--    ALTER TABLE `TournamentStageSet` DROP CONSTRAINT `FK_TournamentStagePool`;
--    ALTER TABLE `DivisionTournamentSet` DROP CONSTRAINT `FK_DivisionDivisionTournament`;
--    ALTER TABLE `TournamentStageSet` DROP CONSTRAINT `FK_DivisionTournamentTournamentStage`;
--    ALTER TABLE `FinalsLinkSet` DROP CONSTRAINT `FK_DivisionFinalsLink`;
--    ALTER TABLE `FieldSet` DROP CONSTRAINT `FK_FieldTournament`;

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------
SET foreign_key_checks = 0;
    DROP TABLE IF EXISTS `TeamSet`;
    DROP TABLE IF EXISTS `PoolSet`;
    DROP TABLE IF EXISTS `DivisionSet`;
    DROP TABLE IF EXISTS `FieldSet`;
    DROP TABLE IF EXISTS `TimeIntervalSet`;
    DROP TABLE IF EXISTS `TournamentSet`;
    DROP TABLE IF EXISTS `MatchSet`;
    DROP TABLE IF EXISTS `TournamentStageSet`;
    DROP TABLE IF EXISTS `DivisionTournamentSet`;
    DROP TABLE IF EXISTS `FinalsLinkSet`;
    DROP TABLE IF EXISTS `TeamMatch`;
    DROP TABLE IF EXISTS `PoolField`;
SET foreign_key_checks = 1;

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

CREATE TABLE `TeamSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Name` longtext NOT NULL, 
	`IsAuto` bool NOT NULL, 
	`PoolPlacement` int, 
	`Pool_Id` int NOT NULL, 
	`PrevPool_Id` int);

ALTER TABLE `TeamSet` ADD PRIMARY KEY (Id);




CREATE TABLE `PoolSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Name` longtext NOT NULL, 
	`IsAuto` bool NOT NULL, 
	`Division_Id` int NOT NULL);

ALTER TABLE `PoolSet` ADD PRIMARY KEY (Id);




CREATE TABLE `DivisionSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Name` longtext NOT NULL, 
	`FieldSize` int NOT NULL, 
	`MatchDuration` int NOT NULL, 
	`TournamentStructure` int NOT NULL, 
	`Tournament_Id` int NOT NULL);

ALTER TABLE `DivisionSet` ADD PRIMARY KEY (Id);




CREATE TABLE `FieldSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Name` longtext NOT NULL, 
	`Size` int NOT NULL, 
	`Tournament_Id` int NOT NULL);

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
	`IsScheduled` bool NOT NULL, 
	`Field_Id` int, 
	`TournamentStage_Id` int NOT NULL);

ALTER TABLE `MatchSet` ADD PRIMARY KEY (Id);




CREATE TABLE `TournamentStageSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`TournamentStructure` int NOT NULL, 
	`IsScheduled` bool NOT NULL, 
	`Pool_Id` int NOT NULL, 
	`DivisionTournament_Id` int NOT NULL);

ALTER TABLE `TournamentStageSet` ADD PRIMARY KEY (Id);




CREATE TABLE `DivisionTournamentSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`TournamentStructure` int NOT NULL, 
	`IsScheduled` bool NOT NULL, 
	`Division_Id` int NOT NULL);

ALTER TABLE `DivisionTournamentSet` ADD PRIMARY KEY (Id);




CREATE TABLE `FinalsLinkSet`(
	`Id` int NOT NULL AUTO_INCREMENT UNIQUE, 
	`Finalstage` int NOT NULL, 
	`PoolPlacement` int NOT NULL, 
	`Division_Id` int NOT NULL);

ALTER TABLE `FinalsLinkSet` ADD PRIMARY KEY (Id);




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

-- Creating foreign key on `Division_Id` in table 'DivisionTournamentSet'

ALTER TABLE `DivisionTournamentSet`
ADD CONSTRAINT `FK_DivisionDivisionTournament`
    FOREIGN KEY (`Division_Id`)
    REFERENCES `DivisionSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DivisionDivisionTournament'

CREATE INDEX `IX_FK_DivisionDivisionTournament` 
    ON `DivisionTournamentSet`
    (`Division_Id`);

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

-- Creating foreign key on `Division_Id` in table 'FinalsLinkSet'

ALTER TABLE `FinalsLinkSet`
ADD CONSTRAINT `FK_DivisionFinalsLink`
    FOREIGN KEY (`Division_Id`)
    REFERENCES `DivisionSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DivisionFinalsLink'

CREATE INDEX `IX_FK_DivisionFinalsLink` 
    ON `FinalsLinkSet`
    (`Division_Id`);

-- Creating foreign key on `Tournament_Id` in table 'FieldSet'

ALTER TABLE `FieldSet`
ADD CONSTRAINT `FK_FieldTournament`
    FOREIGN KEY (`Tournament_Id`)
    REFERENCES `TournamentSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FieldTournament'

CREATE INDEX `IX_FK_FieldTournament` 
    ON `FieldSet`
    (`Tournament_Id`);

-- Creating foreign key on `PrevPool_Id` in table 'TeamSet'

ALTER TABLE `TeamSet`
ADD CONSTRAINT `FK_TeamPrevPool`
    FOREIGN KEY (`PrevPool_Id`)
    REFERENCES `PoolSet`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TeamPrevPool'

CREATE INDEX `IX_FK_TeamPrevPool` 
    ON `TeamSet`
    (`PrevPool_Id`);

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
