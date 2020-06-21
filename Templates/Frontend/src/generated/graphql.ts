export class GraphQLInput {
  operationName: string;
  query: string;
  variables: any;
}

export class GraphQLResponse {
  data: any;
  errors: GraphQLError[];
}

export class GraphQLError {
  message: string;
}