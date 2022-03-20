D365.Data.Helper

Dynamics Crm Web Service Asistant DLL. This library helps you to manipulate data on Dynamics Crm Onpremise or Dynamics 365 Online

<h1>Usage of Dynamics 365 Online</h1>

string connectionStringOnline = "ApiUrl=https://<Organization-Name>.api.crm4.dynamics.com/api/data/v9.1/; ClientId=<Client-Id>;ClientSecret=<Client-Secret>;                                              Resource=https://<Organization-Name>.crm4.dynamics.com/; TenantId=<Tenant-Id>"; //Set connection string
  
  Crm365 crmOnline = new Crm365(connectionStringOnline); //Init connection
  
  IOnlineEntityService onlineEntityService = crmOnline.GetOnlineService();  //Get Online Service
 
  <ul>
    <li>
        GetAllEntititesAsync
        List<Entity> entities = await onlineEntityService.GetAllEntitiesAsync("contact"); //Returns List of Entity 
    </li>  
     <li>    
        GetEntityByIdAsync
        Entity entity = await onlineEntityService.GetEntityByIdAsync("00000000-0000-0000-0000-000000000000", "account"); //Returns Entity
     </li>
     <li>
         CreateEntityAsync
         Entity createEntity = new Entity("contact"); </br> //Build related Entity
         createEntity["firstname"] = "Contact Firstname"; </br>
         createEntity["lastname"] = "Contact Lastname";  </br>
         createEntity["emailaddress1"] = "Contact Email";  </br>
         Guid newId = await onlineEntityService.CreateEntityAsync(createEntity); //Returns Created Entity Id
     </li>
     <li>
          DeleteEntityAsync
          bool isSuccess = await onlineEntityService.DeleteEntityAsync("contact", "00000000-0000-0000-0000-000000000000");//Returns success status
     </li>
    </ul>      
        !! Note if you see an error please contact thanks.
