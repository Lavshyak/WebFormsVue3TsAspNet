`docker-compose up`\
backend: http://localhost:8081, swagger: http://localhost:8081/swagger \
frontend: http://localhost:8083 (nginx with static site)

to run on host:
`cd Backend/WebApi` \
`dotnet run --launch-profile "http"`
http://localhost:8080, swagger: http://localhost:8080/swagger

`cd frontendvue3ts` \
`npm run dev` \
(vite dev server) \
http://localhost:8082

u can configure backend base url in frontend in docker-compose.yaml→sevices→webforms-frontend→buildargs→VITE_BACKEND_URL
or in frontendvue3ts/.env.development→VITE_BACKEND_URL

backend uses c#, .net 8, asp.net 8, Nito.AsyncEx (for async synchronization in repository). \
architecture is more 3-layer rather than DDD. \
backend endpoints:\

- /Forms/Store: \
  handles any JSON content, saves it+Guid+CreatedAt in repository with in-memory DB (List with help of
  AsyncReaderWriterLock)\
- /Forms/Search: \
  handles SearchingConfigurationDto, which can contains in generally a list of paths and values to search in early sent
  JSONs, skip and take counts.

```
 {
   "pathsValues": [
     {
       "path": [
         "formType" // multiple strings if nested
       ],
       "valueSearchingConfigurationDto": {
         "exactStringValue": "productReleases"
       }
     }
   ],
   "skip": 10,
   "take": 5
 }
```

frontend uses TS (it was faster that way), Vue 3, Vite, vuelidate (form validation), kubb (to generate client API from backend's OpenApi), axios (easier to use with kubb).