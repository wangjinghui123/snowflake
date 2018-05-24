# Host: localhost  (Version: 5.7.10)
# Date: 2017-03-08 22:09:27
# Generator: MySQL-Front 5.3  (Build 4.271)

/*!40101 SET NAMES utf8 */;

#
# Structure for table "account"
#

DROP TABLE IF EXISTS `account`;
CREATE TABLE `account` (
  `ID` varchar(255) NOT NULL DEFAULT '',
  `name` varchar(255) DEFAULT NULL,
  `password` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#
# Data for table "account"
#

INSERT INTO `account` VALUES ('1001','andy','1234'),('1002','小红','3333');

#
# Structure for table "player"
#

DROP TABLE IF EXISTS `player`;
CREATE TABLE `player` (
  `name` varchar(255) NOT NULL DEFAULT '',
  `age` int(11) unsigned DEFAULT NULL,
  `sex` int(11) unsigned DEFAULT NULL,
  `level` int(11) unsigned DEFAULT NULL,
  `mvp_count` int(11) unsigned DEFAULT NULL,
  `evolution_level` int(11) unsigned DEFAULT NULL,
  `bean_count` int(11) unsigned DEFAULT NULL,
  `game_count` int(11) unsigned DEFAULT NULL,
  `champion_count` int(11) unsigned DEFAULT NULL,
  `eat_player_count` int(11) unsigned DEFAULT NULL,
  `eat_count` int(11) unsigned DEFAULT NULL,
  `max_weight` int(11) unsigned DEFAULT NULL,
  PRIMARY KEY (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#
# Data for table "player"
#

INSERT INTO `player` VALUES ('andy',17,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),('小红',18,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
