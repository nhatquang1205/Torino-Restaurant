using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TorinoRestaurant.Application.Abstractions.Services;

namespace TorinoRestaurant.Application.Commons.Extensions
{
    /// <summary>
    /// Lớp mở rộng để cấu hình các dịch vụ cần thiết cho 1 microservice
    /// <para>Created at: 22/06/2024</para>
    /// <para>Created by: QuangTN</para>
    /// </summary>
    public static class ServicesExtensions
    {
        /// <summary>
        /// Cấu hình sử dụng xác thực cho API
        /// <para>Created at: 22/06/2024</para>
        /// <para>Created by: QuangTN</para>
        /// </summary>
        /// <param name="services">Các dịch vụ của API hiện tại</param>
        /// <param name="configuration">Các cấu hình API hiện tại</param>
        /// <returns>Dịch vụ sau khi đã cài đặt</returns>
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]))
                };

                // Thêm dịch vụ xác thực vào các dịch vụ hiện tại
                services
                    .AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                    })
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                var accessToken = context.Request.Query["token"];
                                var path = context.HttpContext.Request.Path;
                                if (!string.IsNullOrEmpty(accessToken.ToString()) &&
                                    path.ToString().StartsWith("/hub/"))
                                {
                                    context.Token = accessToken;
                                }
                                return Task.CompletedTask;
                            }
                        };
                        options.TokenValidationParameters = tokenValidationParameters;
                    });
                ;
            }
            catch { }

            return services;
        }

        /// <summary>
        /// Cấu hình cross orgin cho API
        /// <para>Created at: 22/06/2024</para>
        /// <para>Created by: QuangTN</para>
        /// </summary>
        /// <param name="services">Các dịch vụ của API hiện tại</param>
        /// <param name="configuration">Các cấu hình API hiện tại</param>
        /// <param name="corsName">Tên cấu hình cross orgin </param>
        /// <returns>Dịch vụ sau khi đã cài đặt</returns>
        public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration, string corsName = "CorsPolicy")
        {
            services.AddCors(options =>
            {
                options
                    .AddPolicy(corsName,
                        builder => builder
                            .SetIsOriginAllowed((host) => true)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                    );
            });

            return services;
        }

        /// <summary>
        /// Cấu hình sử dụng version cho API
        /// <para>Created at: 22/06/2024</para>
        /// <para>Created by: QuangTN</para>
        /// </summary>
        /// <param name="services">Các dịch vụ của API hiện tại</param>
        /// <param name="configuration">Các cấu hình API hiện tại</param>
        /// <returns>Dịch vụ sau khi đã cài đặt</returns>
        public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                // Thêm api version
                services
                    .AddApiVersioning(options =>
                    {
                        options.ReportApiVersions = true;
                        options.AssumeDefaultVersionWhenUnspecified = true;
                        options.ApiVersionReader = ApiVersionReader.Combine(
                            new QueryStringApiVersionReader(),
                            new HeaderApiVersionReader("x-api-version")
                        );
                        options.DefaultApiVersion = new ApiVersion(1, 0);
                    })
                    ;
            }
            catch { }

            return services;
        }

        /// <summary>
        /// Cấu hình sử dụng Swagger để tạo API doc và test API
        /// <para>Created at: 22/06/2024</para>
        /// <para>Created by: QuangTN</para>
        /// </summary>
        /// <param name="services">Các dịch vụ của API hiện tại</param>
        /// <param name="configuration">Các cấu hình API hiện tại</param>
        /// <param name="serviceName">Tên microservice cần cấu hình</param>
        /// <param name="version">Version của API doc</param>
        /// <returns>Dịch vụ sau khi đã cài đặt</returns>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration, string serviceName, string version = "v1.0")
        {
            try
            {
                services
                    .AddSwaggerGen(options =>
                    {
                        //Set một số thông tin cho API doc
                        options.SwaggerDoc(version, new OpenApiInfo
                        {
                            Title = $"{serviceName} HTTP API",
                            Version = version,
                            Description = $"The {serviceName} Service HTTP API",
                            TermsOfService = null, 
                            Contact = new OpenApiContact 
                            {
                            },
                            License = new OpenApiLicense 
                            {
                            }
                        });

                        //  Nếu có 2 API trùng nhau thì lấy API đầu tiên để đưa vào API doc
                        options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                        string Token = Helpers.GetCookie("token");

                        // Cấu hình xác thực cho API doc
                        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                        {
                            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"" + (String.IsNullOrEmpty(Token) ? "" : (". Curent token: Bearer " + Token)),
                            Name = "Authorization",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.ApiKey,
                            Scheme = "Bearer"
                        });

                        // options.OperationFilter<AuthorizeCheckOperationFilter>();
                        var xmlFile = $"TorinoRestaurant.Application.xml";
                        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                        options.IncludeXmlComments(xmlPath);
                        options.OperationFilter<SwaggerParameterFilter>();
                        options.CustomSchemaIds(x => x.FullName);
                    }
                ).AddSwaggerGenNewtonsoftSupport();
            }
            catch { }

            return services;
        }

        /// <summary>
        /// Cấu hình sử dụng các biến ở appSetting
        /// <para>Created at: 29/06/2024</para>
        /// <para>Created by: QuangTN</para>
        /// </summary>
        /// <param name="services">Các dịch vụ của API hiện tại</param>
        /// <param name="configuration">Các cấu hình API hiện tại</param>
        /// <returns>Dịch vụ sau khi đã cài đặt</returns>
        public static IServiceCollection AddCustomOption(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                services.AddOptions<JwtSettings>()
                    .BindConfiguration($"{nameof(JwtSettings)}")
                    .ValidateDataAnnotations()
                    .ValidateOnStart();
                
                services.AddOptions<MinioSettings>()
                    .BindConfiguration($"{nameof(MinioSettings)}")
                    .ValidateDataAnnotations()
                    .ValidateOnStart();
                
            }
            catch { }

            return services;
        }
    }

    /// <summary>
    /// Thuộc tính để thêm vào property để bỏ qua khi render swagger UI
    /// <para>Created at: 08/08/2020</para>
    /// <para>Created by: QuyPN</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SwaggerExcludeAttribute : Attribute
    {
    }

    /// <summary>
    /// Thuộc tính để thêm vào property để tạo combobox từ code master
    /// <para>Created at: 08/08/2020</para>
    /// <para>Created by: QuyPN</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CodeMasterAttribute : Attribute
    {
        private readonly Type t;
        public CodeMasterAttribute(Type type)
        {
            t = type;
        }
        public Type Type
        {
            get
            {
                return t;
            }
        }
    }

    /// <summary>
    /// Lọc các tham số của API sẽ render trên UI, bỏ qua các tham số trên dường dẫn (api_version) và thêm các giá trị mặc định cho các tham số
    /// <para>Created at: 22/06/2024</para>
    /// <para>Created by: QuangTn</para>
    /// </summary>
    public class SwaggerParameterFilter : IOperationFilter
    {
        /// <summary>
        /// Ghi đè phương thức để lọc tham số
        /// <para>Created at: 22/06/2024</para>
        /// <para>Created by: QuangTn</para>
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        void IOperationFilter.Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Lấy các tham số cần bỏ qua
            var ignoredProperties = context.MethodInfo.GetParameters()
                .SelectMany(p => p.ParameterType.GetProperties().Where(prop => prop.GetCustomAttribute<SwaggerExcludeAttribute>() != null));

            if (ignoredProperties.Any())
            {
                // Nếu có tham số cần bỏ qua thì tiến hành xoá khỏi các tham số sẽ render
                foreach (var property in ignoredProperties)
                {
                    operation.Parameters = operation.Parameters.Where(p => !p.Name.Equals(property.Name, StringComparison.InvariantCulture)).ToList();
                }
            }

            // Lấy các tham số có data từ code master
            var codeMasterProperties = context.MethodInfo.GetParameters()
                .SelectMany(p => p.ParameterType.GetProperties().Where(prop => prop.GetCustomAttribute<CodeMasterAttribute>() != null));
            if (codeMasterProperties.Any())
            {
                // Nếu có tham số cần chuyển thành enum thì tiến hành chuyển đổi
                foreach (var property in codeMasterProperties)
                {
                    var parameter = operation.Parameters.FirstOrDefault(p => p.Name.Equals(property.Name, StringComparison.InvariantCulture));
                    if (parameter != null)
                    {
                        var attr = property.GetCustomAttribute<CodeMasterAttribute>();
                        if (attr != null)
                        {
                            foreach (var type in attr.Type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic))
                            {
                                if (!type.IsAbstract)
                                {
                                    continue;
                                }
                                string key = "";
                                string value = "";
                                foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                                {
                                    if (field.Name == "CODE")
                                    {
                                        key = field.GetValue(null).ToString();
                                    }
                                    if (field.Name == "NAME")
                                    {
                                        value = field.GetValue(null).ToString();
                                    }
                                }
                                if (key != "")
                                {
                                    parameter.Schema.Enum.Add(new OpenApiString(key));
                                    parameter.Description += $"\n{key}: {value}";
                                }
                            }
                        }
                    }
                }
            }

            // Xoá tham số api version trên dường dẫn
            operation.Parameters = operation.Parameters.Where(p => !p.Name.Equals("v", StringComparison.InvariantCulture) && !p.Name.Equals("api_version", StringComparison.InvariantCulture)).ToList();

            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            // Thêm tham số là api version với giá trị mặc định
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "api_version",
                Description = "Version của API cần gọi",
                In = ParameterLocation.Path,
                Schema = new OpenApiSchema() { Type = "string", Default = new OpenApiString("1.0") },
                Required = true
            });

            // Thêm tham số là x-requestid với giá trị mặc định
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "x-requestid",
                Description = "Id để định danh cho request này (mỗi lần request hãy truyền một uuid khác nhau)",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema() { Type = "string", Default = new OpenApiString("593a8bfb-f53e-42ad-ae96-75e2ae803f1a") },
                Required = true
            });

            // Thêm tham số là x-apikey với giá trị mặc định
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "x-apikey",
                Description = "API key để xác thực nguồn gọi API",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema() { Type = "string", Default = new OpenApiString("U2Nqgc3HNDyHG7263mxER9nHfhY7ssREfCUEDHM4PvKgt3Wj") },
                Required = true
            });

            if (operation.Security == null)
                operation.Security = new List<OpenApiSecurityRequirement>();

            var scheme = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } };
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [scheme] = new List<string>()
            });
        }
    }
}