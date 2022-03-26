D365.Data.Helper

Dynamics Crm Web Service Asistant DLL. This library helps you to manipulate data on Dynamics Crm Onpremise or Dynamics
365 Online

<h1>Usage of Dynamics 365 Online</h1>
string connectionStringOnline = "ApiUrl=https://<Organization-Name>.api.crm4.dynamics.com/api/data/v9.1/; ClientId=<Client-Id>;ClientSecret=<Client-Secret>; Resource=https://<Organization-Name>.crm4.dynamics.com/; TenantId=<Tenant-Id>"; //Set connection string

                    Crm365 crmOnline = new Crm365(connectionStringOnline); //Init connection

                    IOnlineEntityService onlineEntityService = crmOnline.GetOnlineService(); //Get Online Service

                    <ul>
                        <li>
                            <h3>GetAllEntititesAsync</h3>
                            List<Entity> entities = await onlineEntityService.GetAllEntitiesAsync("contact"); //Returns
                                List of Entity
                        </li>
                        <li>
                            <h3>GetEntityByIdAsync</h3>
                            Entity entity = await
                            onlineEntityService.GetEntityByIdAsync("00000000-0000-0000-0000-000000000000", "account");
                            //Returns Entity
                        </li>
                        <li>
                            <h3>GetEntityByIdAsync</h3>
                            Entity entity = await
                            onlineEntityService.GetEntityByIdAsync("00000000-0000-0000-0000-000000000000", "account",new
                            string[]{"accountid"}); //Returns Entity with expected columns
                        </li>
                        <li>
                            <h3>CreateEntityAsync</h3>
                            Entity createEntity = new Entity("contact"); //Build related Entity </br>
                            createEntity["firstname"] = "Contact Firstname"; </br>
                            createEntity["lastname"] = "Contact Lastname"; </br>
                            createEntity["emailaddress1"] = "Contact Email"; </br>
                            Guid newId = await onlineEntityService.CreateEntityAsync(createEntity); //Returns Created
                            Entity Id
                        </li>
                        <li>
                            <h3>DeleteEntityAsync</h3>
                            bool isSuccess = await onlineEntityService.DeleteEntityAsync("contact",
                            "00000000-0000-0000-0000-000000000000");//Returns success status
                        </li>
                        <li>
                            <h3>UpdateEntityAsync</h3>
                            Entity updateEntity = new Entity("contact"); //Build related Entity </br>
                            updateEntity = Guid.Parse("00000000-0000-0000-0000-000000000000");// record id
                            updateEntity["firstname"] = "Contact Firstname"; </br>
                            updateEntity["lastname"] = "Contact Lastname"; </br>
                            updateEntity["emailaddress1"] = "Contact Email"; </br bool isSuccess=await
                                onlineEntityService.UpdateEntityAsync(updateEntity); //Returns success status </li>
                        </li>
                        <li>
                            <h3>GetEntitiesByFetchXmlAsync</h3>
                            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical'
                                distinct='false'>
                                <entity name='contact'>
                                    <attribute name='fullname' />
                                    <attribute name='telephone1' />
                                    <attribute name='contactid' />
                                    <order attribute='fullname' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='new_mailfileisuploaded' operator='eq' value='0' />
                                    </filter>
                                </entity>
                            </fetch>
                            ";
                            List<Entity> entities = await onlineEntityService.GetEntitiesByFetchXmlAsync("contact",
                                fetchXml);

                        </li>
                    </ul>
                    
<h1>Usage of Dynamics 365 Onpremise</h1>
string connectionStringOnPremise = "ApiUrl=http://<Crm-Server-Ip>/api/data/v9.0/;  UserName =<Crm-Username>; Password =<Crm-Password>;";//Set connection string
                                       

                                        Crm365 crmOnpremise = new Crm365(connectionStringOnPremise); //Init connection

                                        IOnPremiseEntityService onPremiseEntityService = crmOnpremise.GetOnPremiseService(); //Get
                                        Onpremise Service

                                        <ul>
                                            <li>
                                                <h3>GetAllEntititesAsync</h3>
                                                List<Entity> entities = await
                                                    onPremiseEntityService.GetAllEntitiesAsync("contact"); //Returns List
                                                    of Entity
                                            </li>
                                            <li>
                                                <h3>GetEntityByIdAsync</h3>
                                                Entity entity = await
                                                onPremiseEntityService.GetEntityByIdAsync("00000000-0000-0000-0000-000000000000",
                                                "account"); //Returns Entity
                                            </li>
                                            <li>
                                                <h3>GetEntityByIdAsync</h3>
                                                Entity entity = await
                                                onPremiseEntityService.GetEntityByIdAsync("00000000-0000-0000-0000-000000000000",
                                                "account",new string[]{"accountid"}); //Returns Entity with expected
                                                columns
                                            </li>
                                            <li>
                                                <h3>CreateEntityAsync</h3>
                                                Entity createEntity = new Entity("contact"); //Build related Entity
                                                </br>
                                                createEntity["firstname"] = "Contact Firstname"; </br>
                                                createEntity["lastname"] = "Contact Lastname"; </br>
                                                createEntity["emailaddress1"] = "Contact Email"; </br>
                                                Guid newId = await onPremiseEntityService.CreateEntityAsync(createEntity);
                                                //Returns Created Entity Id
                                            </li>
                                            <li>
                                                <h3>DeleteEntityAsync</h3>
                                                bool isSuccess = await onPremiseEntityService.DeleteEntityAsync("contact",
                                                "00000000-0000-0000-0000-000000000000");//Returns success status
                                            </li>
                                            <li>
                                                <h3>UpdateEntityAsync</h3>
                                                Entity updateEntity = new Entity("contact"); //Build related Entity</br>
                                                updateEntity = Guid.Parse("00000000-0000-0000-0000-000000000000");//
                                                record id
                                                updateEntity["firstname"] = "Contact Firstname"; </br>
                                                updateEntity["lastname"] = "Contact Lastname"; </br>
                                                updateEntity["emailaddress1"] = "Contact Email"; </br 
                                                bool isSuccess=await onPremiseEntityService.UpdateEntityAsync(updateEntity); </li>
                                            </li>

                                            <li>
                                                <h3>GetEntitiesByFetchXmlAsync</h3>
                                                string fetchXml = @"<fetch version='1.0' output-format='xml-platform'
                                                    mapping='logical' distinct='false'>
                                                    <entity name='contact'>
                                                        <attribute name='fullname' />
                                                        <attribute name='telephone1' />
                                                        <attribute name='contactid' />
                                                        <order attribute='fullname' descending='false' />
                                                    </entity>
                                                </fetch>
                                                ";
                                                List<Entity> entities = await onPremiseEntityService.GetEntitiesByFetchXmlAsync("contact", fetchXml);

                                            </li>
                                        </ul>

                                        !! Note if you see an error please contact thanks.
