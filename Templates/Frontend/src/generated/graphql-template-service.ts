import { Injectable } from '@angular/core';

const MUTATION_TEMPLATE: string = "mutation {\n\
    {action}({mutation}) {\n\
      {ouput}\
    }\
  \n}\n";

const QUERY_TEMPLATE: string = "query {\n\
    {action} {query} {\n\
    {ouput}\
    }\
  \n}\n";

export class QueryParameter {
    name: string;
    comparison: Comparison;
    value: string;
}

export enum Comparison {
    Equals,
    Contains
}

@Injectable({
    providedIn: 'root'
})
export class GraphQLTemplateService {

    public convertToMutation(action: string, outputDto: any, parameterName: string, fields: string[]): string {

        let template: string = MUTATION_TEMPLATE;
        template = template.replace("{action}", action);

        let useFields = fields !== null && fields.length > 0;
        var name = parameterName === null ? outputDto.constructor.name.toLowerCase() : parameterName;
        let input: string = name + ": {";
        let query: string = useFields ? fields.join("\n") : "";

        for (let p in outputDto) {

            let val = outputDto[p];

            if (typeof val === "boolean") {
                input += p + ": " + val + ", ";
            } else if (typeof val === "number") {
                input += p + ": " + val + ", ";
            } else {
                input += p + ": \"" + val + "\", ";
            }

            if (useFields === false) query += p + "\n";
        }

        if (input.endsWith(", ")) {
            input = input.substr(0, input.length - 2);
        }

        input += " }";

        template = template.replace("{mutation}", input);
        template = template.replace("{ouput}", query);

        return template;
    }

    private getCompareString(comparison: Comparison): string {
        if (comparison === Comparison.Contains) {
            return "contains:";
        }

        if (comparison === Comparison.Equals) {
            return "equal:";
        }

        throw new Error("Not implemented.");
    }

    public convertToQuery(action: string, queries: QueryParameter[], fields: string[]): string {

        let template: string = QUERY_TEMPLATE;
        template = template.replace("{action}", action);

        let query: string = "";
        if (queries !== null && queries.length > 0) {

            query += "("
            queries.forEach(qp => {

                let compare = this.getCompareString(qp.comparison);
                query += qp.name + ": {" + compare + "\"" + qp.value + "\"" + "},";
            });

            if (query.endsWith(",")) {
                query = query.substr(0, query.length - 1);
            }

            query += ")"
        }

        template = template.replace("{query}", query);

        template = template.replace("{ouput}", fields.join("\n"));

        return template;
    }
}
