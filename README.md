D365.Data.Helper

Dynamics Crm Web Service Asistant DLL. This library helps you to manipulate data on Dynamics Crm Onpremise or Dynamics
365 Online

Usage of Dynamics 365 Online

            string connectionStringOnline = "ApiUrl=https://Organization-Name.api.crm4.dynamics.com/api/data/v9.1/; ClientId=Client-Id;ClientSecret=Client-Secret; Resource=https://Organization-Name.crm4.dynamics.com/; TenantId=Tenant-Id"; //Set connection string

            Crm365 crmOnline = new Crm365(connectionStringOnline); //Init connection

            IOnlineEntityService onlineEntityService = crmOnline.GetOnlineService(); //Get Online Service

                *GetAllEntititesAsync
                List<Entity> entities = await onlineEntityService.GetAllEntitiesAsync("contact"); //Returns
                    List of Entity

                *GetEntityByIdAsync
                Entity entity = await
                onlineEntityService.GetEntityByIdAsync("00000000-0000-0000-0000-000000000000", "account");
                //Returns Entity

                *GetEntityByIdAsync
                Entity entity = await
                onlineEntityService.GetEntityByIdAsync("00000000-0000-0000-0000-000000000000", "account",new
                string[]{"accountid"}); //Returns Entity with expected columns

                *CreateEntityAsync
                Entity createEntity = new Entity("contact"); //Build related Entity
                createEntity["firstname"] = "Contact Firstname";
                createEntity["lastname"] = "Contact Lastname";
                createEntity["emailaddress1"] = "Contact Email";
                Guid newId = await onlineEntityService.CreateEntityAsync(createEntity); //Returns Created
                Entity Id

                *DeleteEntityAsync
                bool isSuccess = await onlineEntityService.DeleteEntityAsync("contact",
                "00000000-0000-0000-0000-000000000000");//Returns success status

                *UpdateEntityAsync
                Entity updateEntity = new Entity("contact"); //Build related Entity
                updateEntity = Guid.Parse("00000000-0000-0000-0000-000000000000");// record id
                updateEntity["firstname"] = "Contact Firstname";
                updateEntity["lastname"] = "Contact Lastname";
                updateEntity["emailaddress1"] = "Contact Email";
                bool isSuccess=await onlineEntityService.UpdateEntityAsync(updateEntity); //Returns success
                status

                *GetEntitiesByFetchXmlAsync
                string fetchXml = @"fetchXML Query";
                await onlineEntityService.GetEntitiesByFetchXmlAsync("contact",fetchXml);
                //Returns List of Entity


Usage of Dynamics 365 Onpremise

                string connectionStringOnPremise = "ApiUrl=http://<Crm-Server-Ip>/api/data/v9.0/; UserName=<Crm-Username>; Password =<Crm-Password>;";//Set connection string

                Crm365 crmOnpremise = new Crm365(connectionStringOnPremise); //Init connection

                IOnPremiseEntityService onPremiseEntityService = crmOnpremise.GetOnPremiseService();
                //Get Onpremise Service

                    *GetAllEntititesAsync
                    List<Entity> entities = await
                        onPremiseEntityService.GetAllEntitiesAsync("contact"); //Returns List
                        of Entity

                    *GetEntityByIdAsync
                    Entity entity = await
                    onPremiseEntityService.GetEntityByIdAsync("00000000-0000-0000-0000-000000000000",
                    "account"); //Returns Entity

                    *GetEntityByIdAsync
                    Entity entity = await
                    onPremiseEntityService.GetEntityByIdAsync("00000000-0000-0000-0000-000000000000",
                    "account",new string[]{"accountid"}); //Returns Entity with expected
                    columns

                    *CreateEntityAsync
                    Entity createEntity = new Entity("contact"); //Build related Entity

                    createEntity["firstname"] = "Contact Firstname";
                    createEntity["lastname"] = "Contact Lastname";
                    createEntity["emailaddress1"] = "Contact Email";
                    Guid newId = await onPremiseEntityService.CreateEntityAsync(createEntity);
                    //Returns Created Entity Id

                    *DeleteEntityAsync
                    bool isSuccess = await onPremiseEntityService.DeleteEntityAsync("contact",
                    "00000000-0000-0000-0000-000000000000");//Returns success status

                    *UpdateEntityAsync
                    Entity updateEntity = new Entity("contact"); //Build related Entity
                    updateEntity = Guid.Parse("00000000-0000-0000-0000-000000000000");//
                    record id
                    updateEntity["firstname"] = "Contact Firstname";
                    updateEntity["lastname"] = "Contact Lastname";
                    updateEntity["emailaddress1"] = "Contact Email";
                    bool isSuccess=await onPremiseEntityService.UpdateEntityAsync(updateEntity);

                    *GetEntitiesByFetchXmlAsync
                    string fetchXml = @"fetchXML Query";
                    await onPremiseEntityService.GetEntitiesByFetchXmlAsync("contact",fetchXml);
                    //Returns List of Entity

                    !! Note if you see an error please contact thanks.
