using HRMS.API.Middleware;
using HRMS.API.RegisterDependencies;
using HRMS.Shared.Application.Extensions;
using HRMS.Shared.Application.GraphQL;
using IdentityFeature.GraphQL;
using OnboardingFeature.GraphQL;
using AttendanceFeature.GraphQL;
using LeaveFeature.GraphQL;
using PayrollFeature.GraphQL;
using DocumentFeature.GraphQL;
using ExpenseFeature.GraphQL;
using PerformanceFeature.GraphQL;
using TrainingFeature.GraphQL;
using RecruitmentFeature.GraphQL;
using RecognitionFeature.GraphQL;
using AnnouncementFeature.GraphQL;
using AnalyticsFeature.GraphQL;
using CopilotFeature.GraphQL;
using ContributionsFeature.GraphQL;
using HotChocolate;
using HotChocolate.Types;

namespace HRMS.API.Extensions
{
    public static class GraphQLExtensions
    {
        public static void ConfigureGraphQL(this IServiceCollection services, IConfiguration configuration)
        {
            bool allowIntrospection = configuration.GetValue<bool>("GraphQL:AllowIntrospection", false);
            bool includeExceptionDetails = configuration.GetValue<bool>("GraphQL:IncludeExceptionDetails", false);
            int requestTimeoutSeconds = configuration.GetValue<int>("RequestTimeout:Seconds", 90);

            services.AddGraphQLServer()
                    .ConfigureSchemaServices(schemaServices =>
                    {
                        schemaServices.AddHttpContextAccessor();
                        schemaServices.AddLogging();
                    })
                    .ModifyRequestOptions(o =>
                    {
                        o.IncludeExceptionDetails = includeExceptionDetails;
                        o.ExecutionTimeout = TimeSpan.FromSeconds(requestTimeoutSeconds);
                    })
                    .AddAuthorization()
                    .ModifyParserOptions(opt =>
                    {
                        opt.MaxAllowedFields = 4096; // ? increase from default 2048
                    })
                    .AddQueryType(d => d.Name("Query"))
                    .AddMutationType(d => d.Name("Mutation"))
                    .AddIdentityGraphQL()
                    .AddOnboardingGraphQL()
                    .AddAttendanceGraphQL()
                    .AddLeaveGraphQL()
                    .AddPayrollGraphQL()
                    .AddDocumentGraphQL()
                    .AddExpenseGraphQL()
                    .AddPerformanceGraphQL()
                    .AddTrainingGraphQL()
                    .AddRecruitmentGraphQL()
                    .AddRecognitionGraphQL()
                    .AddAnnouncementGraphQL()
                    .AddAnalyticsGraphQL()
                    .AddCopilotGraphQL()
                    .AddContributionGraphQL()
                    .AddErrorFilter<GraphQLErrorFilter>()
                    .AddType<UploadType>()
                    .AddInMemorySubscriptions()
                    .AddGraphQLModules();

            services.Configure<SchemaOptions>(options =>
            {
                options.EnableDirectiveIntrospection = allowIntrospection;
            });
        }
    }
}