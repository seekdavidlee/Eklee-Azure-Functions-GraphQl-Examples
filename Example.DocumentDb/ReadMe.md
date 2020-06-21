# Introduction

This example demostrates the use of the DocumentDb resolver for handling your Models. 

## Setup

Create a local.settings.json file with the following content to start it. Be sure to start the Azure CosmosDB Emulator first.

```
{
    "IsEncrypted": false,
	"Host": {
		"LocalHttpPort": 7071,
		"CORS": "*"
	},
	"Values": {
		"AzureWebJobsStorage": "",
		"FUNCTIONS_WORKER_RUNTIME": "dotnet",
		"Db:Name": "sampledb",
		"Db:Url": "https://localhost:8081",
		"Db:Key": "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
		"Db:RequestUnits": "400"
	}
}
```

## Build end-to-end example project

A full end-to-end GraphQL implementation includes the frontend and backend. Here, we have a command to generate a sample project so to help get familiar with the various moving parts.

```
.\GenSolution.ps1 -Name Eklee-SampleApp -OutputPath C:\dev -ExampleType DocumentDB -addBulma
```

The command will take several minutes to complete. After that, you can launch the Api Project with Visual Studio. You can run the Api project. Next, ensure the Cosmos DB emulator is running. Use the following command to generate graphql typescript file which is generated from the schema of the API.

```
npx graphql-codegen --config ./codegen.yml
```

We will now add the url to the backend in the environment.ts file located under src/environments folder.

```
export const environment = {
  production: false,
  url: "http://localhost:7071/api/testdocumentdb"
};
```

Next, we will create a repository service to connect to the backend.

```
npm run ng generate service product-repo
```

Replace the content of the service file with the following sample code which has 2 methods, one for adding a Product, and the other for listing all products. Notice HttpGraphQLService which is a generic GraphQL client service you can use for calling the backend. GraphQLTemplateService is used for generating the appropriate GraphQL queries.

```
import { Injectable } from '@angular/core';
import { HttpGraphQLService, GraphQLParameters } from 'src/generated/graphql-http-service';
import { GraphQLTemplateService } from 'src/generated/graphql-template-service';
import { Product } from 'src/generated/graphql-types';
import { Observable } from 'rxjs';
import { GraphQLInput } from 'src/generated/graphql';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductRepoService {

  constructor(private http: HttpGraphQLService,
    private graphqlTemplateService: GraphQLTemplateService) { }

  list(): Observable<Product[]> {
    let input = new GraphQLInput();

    input.operationName = "";
    input.query = this.graphqlTemplateService.convertToQuery("getAllProducts", null, ["id", "name"]);
    input.variables = {};

    let param = new GraphQLParameters();
    param.url = environment.url;
    param.graphInput = input;
    param.typeName = "getAllProducts";

    return this.http.postAsync(param);
  }

  add(product: Product): Observable<Product> {

    let input = new GraphQLInput();

    input.operationName = "";
    input.query = this.graphqlTemplateService.convertToMutation("createProduct", product, "product", null);
    input.variables = {};

    let param = new GraphQLParameters();
    param.url = environment.url;
    param.graphInput = input;
    param.typeName = "createProduct";

    return this.http.postAsync(param);
  }
}

```

Replace content for src\app\app.component.html with the following to show UI for listing and adding product.

```
<section class="section">
    <div class="container">

        <div class="level">
            <div class="level-left">
            </div>
            <div class="level-right">
                <a class="level-item button is-info" (click)="add()">Add</a>
            </div>
        </div>
        <table class="table is-striped">
            <thead>
                <tr>
                    <th><abbr title="Text">Id</abbr></th>
                    <th><abbr title="Text">Name</abbr></th>
                </tr>
            </thead>
            <tbody>
                <tr *ngFor="let product of products">
                    <td> {{product.id}} </td>
                    <td> {{product.name}} </td>
                </tr>
            </tbody>
        </table>
    </div>
</section>

<div class="modal" [ngClass]="{'is-active': showAdd===true}">
    <div class="modal-background"></div>

    <div class="modal-card model-card-small">
        <header class="modal-card-head">
            <p class="modal-card-title">Add</p>
            <button class="delete" aria-label="close" (click)="showAdd = false"></button>
        </header>

        <section class="modal-card-body">
            <div class="field">
                <label class="label">Name</label>
                <div class="control">
                    <input [(ngModel)]="product.name" class="input" type="text" placeholder="e.g Product name">
                </div>
            </div>
        </section>

        <footer class="modal-card-foot">
            <button class="button is-success" [ngClass]="{'is-loading': isSaving === true}" (click)="saveChanges()">Save
                changes</button>
            <button class="button" (click)="showAdd = false">Cancel</button>
        </footer>
    </div>
</div>
```

Replace the content for src\app\app.component.ts with the following.

```
import { Component, OnInit } from '@angular/core';
import { ProductRepoService } from './product-repo.service';
import { Product } from 'src/generated/graphql-types';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(private productRepoSvc: ProductRepoService) { }

  products: Product[];
  showAdd: boolean;
  isSaving: boolean;
  product: Product = this.newProduct();

  add(): void {
    this.product = this.newProduct();

    this.showAdd = true;
  }

  newProduct(): Product {
    return {
      id: Math.round((new Date()).getTime() / 1000).toString(),
      name: "", category: "", costPrice: 0.0,
      description: "none", sellPrice: 0.0
    };
  }

  saveChanges(): void {
    this.showAdd = false;

    this.productRepoSvc.add(this.product).subscribe(x => {
      this.reload();
    }, err => {
      alert(err);
    })
  }

  cancel(): void {
    this.showAdd = false;
  }

  reload(): void {
    this.productRepoSvc.list().subscribe(x => {
      this.products = x;
    }, err => {
      alert(err);
    });
  }

  ngOnInit(): void {
    this.reload();
  }
}

```

We will need to import the appropriate modules. Replace the content for src\app\app.module.ts with the following.

```
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
```

Now, we are ready to run the frontend code.

```
npm run start
```

If everything works, you can simply browse to http://localhost:4200/. You can click on Add to add new products, and the list will be updated.

## Load test

Use the following command to run a load test from the root directory.

```
.\Util\RunLoadTest.ps1 -Name Example.DocumentDb -ApiName testdocumentdb -TestFileName ProductsOnly2.json -OutputReportDir C:\dev\loadtest-reports
```