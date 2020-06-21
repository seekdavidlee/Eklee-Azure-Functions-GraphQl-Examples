import { HttpClient } from '@angular/common/http';
//import { MsAdalAngular6Service } from 'microsoft-adal-angular6';
import { GraphQLInput, GraphQLResponse, GraphQLError } from './graphql';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

export class GraphQLParameters {
    url: string;
    typeName: string;
    graphInput: GraphQLInput;
    config: AdalConfig;
}

export class AdalConfig {
    clientId: string;
}

@Injectable({
    providedIn: 'root'
})
export class HttpGraphQLService {

    constructor(
        private http: HttpClient,
        //private adalSvc: MsAdalAngular6Service
        ) { }

    postAsync<TOutput>(graphQLParameters: GraphQLParameters): Observable<TOutput> {

        return new Observable<TOutput>((observable) => {

            var handleError = (err: any) => {
                observable.error(err);
            };

            var handleSuccess = (result: GraphQLResponse) => {


                if (result.errors) {
                    if (result.errors.length > 0) {
                        let errors = <GraphQLError[]>result.errors;
                        errors.forEach(x => observable.error(x.message));
                        return;
                    }
                }
                if (result.data) {
                    var raw = result.data[graphQLParameters.typeName];
                    if (raw !== null) {
                        let dto: TOutput = <TOutput>raw;
                        observable.next(dto);
                        observable.complete();
                        return;
                    }
                }
                observable.next(null);
                observable.complete();
            };

            //let endpoint: string = disableAdal ? environment.graphQL.readonlyEndpoint : environment.graphQL.endpoint;
            let input = JSON.stringify(graphQLParameters.graphInput);

            if (!graphQLParameters.config) {

                this.http.post(graphQLParameters.url, input, {
                    headers: {
                        "Content-Type": "application/json"
                    }
                }).subscribe(handleSuccess, handleError);

            } else {
                /*
                this.adalSvc.acquireToken(graphQLParameters.config.clientId).subscribe((token: string) => {

                    this.http.post(graphQLParameters.url, input, {
                        headers: {
                            authorization: "Bearer " + token,
                            "Content-Type": "application/json"
                        }
                    }).subscribe(handleSuccess, handleError);

                }, handleError);
                */
            }
        });
    }
}