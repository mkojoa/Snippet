
select * from HCM_Application_Admin.dbo.AspNetUsers

  select * from psHCMEmployee where szLastName LIKE '%FRENCH-BAIDOO%'

  select * from psHCMEmployee where pkId = '34665FDB-CC14-4BCD-A218-2B3705239F3C' --companyId = D5DF835A-D683-44FF-B351-0EE7056ACB59
  select * from psHCMEmpOrgInfo where uEmployeeid = '34665FDB-CC14-4BCD-A218-2B3705239F3C'

  select * from [psHREmployeeMovement] where pkId = ''

  select * from psHCMSalaryGrade where iCompanyId = 'D5DF835A-D683-44FF-B351-0EE7056ACB59'

  select * from psHCMCodes where szType = 'NTY' where  iCompanyId ='D5DF835A-D683-44FF-B351-0EE7056ACB59'

  UPDATE psHCMOrgCodes set iCompanyId ='D5DF835A-D683-44FF-B351-0EE7056ACB59' where iStatus = 0

  select * from psHCMCodes where pkId IN('52189306-2B7A-44BF-BA67-0B43AC6D674D')

