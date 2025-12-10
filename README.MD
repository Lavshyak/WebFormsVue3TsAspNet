PART 1.

![Screenshot of web site.](https://github.com/Lavshyak/WebFormsVue3TsAspNet/blob/master/2025-12-07%2021-39-54.png)

1 - done (ProductSubmittingForm.vue) \
2 - done (with vuelidate) \
3 - done; a - done,
b - done (handles any JSON),
c - done (repository with List with async multithread synchronization) \
4 - done (frontend - vue 3 app (and static site with nginx); swagger on backend side) \
5 - done (Vite with cssInjectedByJsPlugin).

`docker-compose up`\
backend: http://localhost:8081, swagger: http://localhost:8081/swagger \
frontend: http://localhost:8083 (nginx with static site)

to run on host: \
`cd Backend/WebApi && ` \
`dotnet run --launch-profile "http"`
http://localhost:8080, swagger: http://localhost:8080/swagger

`cd frontendvue3ts` \
`npm i` \
`npm run dev` \
(vite dev server) \
http://localhost:8082

u can configure backend base url in frontend in docker-compose.yaml→sevices→webforms-frontend→buildargs→VITE_BACKEND_URL
or in frontendvue3ts/.env.development→VITE_BACKEND_URL

backend uses c#, .net 8, asp.net 8, Nito.AsyncEx (for async synchronization in repository). \
architecture is more 3-layer rather than DDD. \
backend endpoints:

- /Forms/Store: \
  handles any JSON content, saves it+Guid+CreatedAt in repository with in-memory DB (List with help of
  AsyncReaderWriterLock)\
- /Forms/Search: \
  handles SearchingConfigurationDto, which can contain in generally a list of paths and values to search in early sent
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

frontend uses TS (it was faster that way to develop), Vue 3, Vite, vuelidate (form validation), kubb (to generate client API from backend's OpenApi), axios (easier to use with kubb).

PART 2.

client can send JSON to POST /submitForm endpoint:
```
{
someTextProp: "yoo this is message on forum, check the photo of my phone: !attachment_1;"
attachments: [
{
"localId": "attachment_1",
"plannedSizeInBytes": 91168468
}
]
}
```
backend responds with JSON:
```
[
{
"attachmentLocalId": "attachment_1",
"linkForUploadBinaryAttachmentOnForExampleObjectStorage_YandexOrMinio": "https://superobjectstorage/upload?id=hjeiu73wtw44bjh"
 // auth token can be contained in the link or in this json node and will then be passed in the request header by client.
}
]
```
then client can upload his file to PUT https://superobjectstorage/upload?id=hjeiu73wtw44bjh with Content-Type: multipart/form-data for example.
