/*
SQLyog Ultimate v12.08 (64 bit)
MySQL - 5.7.22 : Database - knowledge_applet
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`knowledge_applet` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `knowledge_applet`;

/*Table structure for table `localmodelinfo` */

CREATE TABLE `localmodelinfo` (
  `id` varchar(50) NOT NULL COMMENT 'id',
  `userId` varchar(50) DEFAULT NULL COMMENT '用户id',
  `modelId` varchar(50) DEFAULT NULL COMMENT '程序id',
  `modelName` varchar(50) DEFAULT NULL COMMENT '程序名',
  `dirId` varchar(50) DEFAULT NULL COMMENT '程序分类id',
  `dirName` varchar(50) DEFAULT NULL COMMENT '程序分类名',
  `version` varchar(50) DEFAULT NULL COMMENT '程序版本号',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `localmodelinfo` */

insert  into `localmodelinfo`(`id`,`userId`,`modelId`,`modelName`,`dirId`,`dirName`,`version`) values ('2c7ae226-f2a5-4bbc-9d9c-638d73582d14','','b0718b9b-6616-4ac2-a93c-a651c646a774','测试程序',NULL,NULL,'1.0');
insert  into `localmodelinfo`(`id`,`userId`,`modelId`,`modelName`,`dirId`,`dirName`,`version`) values ('36c97fe0-fd6b-44ae-a0f1-392a6db9ce51','003','960425cc-be97-427c-9c80-21bb76b988d5','a',NULL,NULL,'1.0');
insert  into `localmodelinfo`(`id`,`userId`,`modelId`,`modelName`,`dirId`,`dirName`,`version`) values ('8d0b922e-9f68-4f99-a663-d8b148df930c','003','c4d9932b-ff05-4d94-9226-e61818f45554',NULL,NULL,NULL,'1.0');
insert  into `localmodelinfo`(`id`,`userId`,`modelId`,`modelName`,`dirId`,`dirName`,`version`) values ('95b89174-5af1-4d8f-8eb9-93039c8e8f11','','a40789de-c806-46ae-93e7-4e85b40c41b7','测试程序','21f1cee8-58b0-41ca-9874-0483266d066f','总体专业','1.0');
insert  into `localmodelinfo`(`id`,`userId`,`modelId`,`modelName`,`dirId`,`dirName`,`version`) values ('9fe64606-fa91-41f5-8746-0b9e01cbdb12','00311','a40789de-c806-46ae-93e7-4e85b40c41b7','测试程序','21f1cee8-58b0-41ca-9874-0483266d066f','总体专业','1.0');
insert  into `localmodelinfo`(`id`,`userId`,`modelId`,`modelName`,`dirId`,`dirName`,`version`) values ('e16efd67-828a-400f-a31b-aaaa93eb16ea','','960425cc-be97-427c-9c80-21bb76b988d5','a',NULL,NULL,'1.0');
insert  into `localmodelinfo`(`id`,`userId`,`modelId`,`modelName`,`dirId`,`dirName`,`version`) values ('e888032d-0ce0-483c-90c9-d563290e0f4d','003','b0718b9b-6616-4ac2-a93c-a651c646a774','测试程序',NULL,NULL,'1.0');

/*Table structure for table `model` */

CREATE TABLE `model` (
  `modelID` varchar(50) NOT NULL COMMENT '程序ID',
  `modelName` varchar(50) DEFAULT NULL COMMENT '程序名',
  `fmiVersion` varchar(50) DEFAULT NULL COMMENT '程序版本号',
  `modelIdentifier` varchar(50) DEFAULT NULL COMMENT '程序标识',
  `guid` varchar(50) NOT NULL COMMENT '程序的Guid编码',
  `description` varchar(2000) DEFAULT NULL COMMENT '程序的简要概述',
  `authorId` varchar(50) DEFAULT NULL COMMENT '程序开发作者id',
  `authorName` varchar(50) DEFAULT NULL COMMENT '程序开发作者名称',
  `generatationTool` varchar(50) DEFAULT NULL COMMENT '程序语言（C\\C++\\C#\\Java\\Python\\Matlab）',
  `programType` varchar(50) DEFAULT NULL COMMENT '程序类型（exe\\dll）',
  `categoryId` varchar(50) DEFAULT NULL COMMENT '程序分类id',
  `categoryName` varchar(50) DEFAULT NULL COMMENT '程序分类（结构、流体）名',
  `copyright` varchar(50) DEFAULT NULL COMMENT '程序版权',
  `remark` varchar(50) DEFAULT NULL COMMENT '备注',
  `downloadNumber` int(11) DEFAULT '0' COMMENT '程序下载数量',
  `applicationSize` double DEFAULT NULL COMMENT '程序大小',
  `publishTime` varchar(50) DEFAULT NULL COMMENT '发布时间',
  `applicationPermissions` int(11) DEFAULT '0' COMMENT '程序权限 0-公开，1-个人，2-部分',
  `isDisable` int(11) DEFAULT '0' COMMENT '是否禁用，0-启用，1-禁用',
  `objectId` varchar(50) NOT NULL COMMENT 'mongodb存文件的objectId',
  PRIMARY KEY (`modelID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `model` */

insert  into `model`(`modelID`,`modelName`,`fmiVersion`,`modelIdentifier`,`guid`,`description`,`authorId`,`authorName`,`generatationTool`,`programType`,`categoryId`,`categoryName`,`copyright`,`remark`,`downloadNumber`,`applicationSize`,`publishTime`,`applicationPermissions`,`isDisable`,`objectId`) values ('960425cc-be97-427c-9c80-21bb76b988d5','a','1.0',NULL,'0003',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,22,NULL,'2018-12-12 11:08:04',0,0,'5c107b947c1eee23c8059c9c');
insert  into `model`(`modelID`,`modelName`,`fmiVersion`,`modelIdentifier`,`guid`,`description`,`authorId`,`authorName`,`generatationTool`,`programType`,`categoryId`,`categoryName`,`copyright`,`remark`,`downloadNumber`,`applicationSize`,`publishTime`,`applicationPermissions`,`isDisable`,`objectId`) values ('b0718b9b-6616-4ac2-a93c-a651c646a774','测试程序','1.0',NULL,'0001',NULL,'003','叶陶',NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,'2018-12-12 10:36:00',0,0,'5c1074107c1eee21c0f99bb2');
insert  into `model`(`modelID`,`modelName`,`fmiVersion`,`modelIdentifier`,`guid`,`description`,`authorId`,`authorName`,`generatationTool`,`programType`,`categoryId`,`categoryName`,`copyright`,`remark`,`downloadNumber`,`applicationSize`,`publishTime`,`applicationPermissions`,`isDisable`,`objectId`) values ('c4d9932b-ff05-4d94-9226-e61818f45554',NULL,'1.0',NULL,'1111',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,'2018-12-12 17:13:32',0,0,'5c10d13cf53c35133cd38382');

/*Table structure for table `modeldir` */

CREATE TABLE `modeldir` (
  `id` varchar(50) NOT NULL COMMENT 'id',
  `name` varchar(50) DEFAULT NULL COMMENT '分类名',
  `description` varchar(100) DEFAULT NULL COMMENT '描述',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `modeldir` */

insert  into `modeldir`(`id`,`name`,`description`) values ('21f1cee8-58b0-41ca-9874-0483266d066f','总体专业','总体专业');

/*Table structure for table `permission` */

CREATE TABLE `permission` (
  `modelId` varchar(50) NOT NULL COMMENT '程序id',
  `depts` varchar(2000) DEFAULT NULL COMMENT '部门',
  `majors` varchar(2000) DEFAULT NULL COMMENT '专业',
  `users` varchar(2000) DEFAULT NULL COMMENT '用户',
  PRIMARY KEY (`modelId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `permission` */

/*Table structure for table `useinfo` */

CREATE TABLE `useinfo` (
  `id` varchar(50) NOT NULL COMMENT 'id',
  `userId` varchar(50) DEFAULT NULL COMMENT '用户id',
  `userName` varchar(50) DEFAULT NULL COMMENT '用户名',
  `modelId` varchar(50) DEFAULT NULL COMMENT '程序id',
  `modelName` varchar(50) DEFAULT NULL COMMENT '程序名',
  `lastUseTime` varchar(50) DEFAULT NULL COMMENT '最后使用时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `useinfo` */

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
