CREATE DATABASE OMS
ON PRIMARY (
    NAME = N'OMS',
    FILENAME = 'C:\Users\Pryce\source\repos\OrderManagementSystem\App_Data\OMS.mdf'
)
LOG ON (
    NAME = N'OMS_log',
    FILENAME = 'C:\Users\Pryce\source\repos\OrderManagementSystem\App_Data\OMS.ldf'
);