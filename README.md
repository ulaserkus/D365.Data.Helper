D365.Data.Helper

Dynamics Crm Web Service Asistant DLL. This library helps you to manipulate data on Dynamics Crm Onpremise or Dynamics 365 Online

Usage of Dynamics 365 Online

string connectionStringOnline = "ApiUrl=https://<Organization-Name>.api.crm4.dynamics.com/api/data/v9.1/; ClientId=<Client-Id>;ClientSecret=<Client-Secret>;                                              Resource=https://<Organization-Name>.crm4.dynamics.com/; TenantId=<Tenant-Id>"; //Set connection string
  
  Crm365 crmOnline = new Crm365(connectionStringOnline); //Init connection
  
  IOnlineEntityService onlineEntityService = crmOnline.GetOnlineService();  //Get Online Service
 
  <ul>
    <li>
    #GetAllEntititesAsync
        List<Entity> entities = await onlineEntityService.GetAllEntitiesAsync("contact"); //Returns List of Entity
    #
     </li>  
        
    #GetEntityByIdAsync
        Entity entity = await onlineEntityService.GetEntityByIdAsync("00000000-0000-0000-0000-000000000000", "account"); //Returns Entity
    #
    
   #CreateEntityAsync
         Entity createEntity = new Entity("contact");  //Build related Entity
         createEntity["firstname"] = "Contact Firstname";
         createEntity["lastname"] = "Contact Lastname";
         createEntity["emailaddress1"] = "Contact Email";

         Guid newId = await onlineEntityService.CreateEntityAsync(createEntity); //Returns Created Entity Id
    #
    
    #DeleteEntityAsync
          bool isSuccess = await onlineEntityService.DeleteEntityAsync("contact", "00000000-0000-0000-0000-000000000000");//Returns success status
    #
    </ul>      
        !! Note if you see an error please contact thanks.
