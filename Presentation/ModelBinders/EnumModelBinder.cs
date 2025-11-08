namespace UltimateDotNetSkeleton.Presentation.ModelBinders
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Serilog;
    using UltimateDotNetSkeleton.Application.Exceptions.BadRequest;

    public class EnumModelBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			ArgumentNullException.ThrowIfNull(bindingContext);

			var modelName = bindingContext.ModelName;
			var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

			if (valueProviderResult == ValueProviderResult.None)
			{
				return Task.CompletedTask;
			}

			bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);
			var value = valueProviderResult.FirstValue;

			if (string.IsNullOrEmpty(value))
			{
				return Task.CompletedTask;
			}

			var modelType = bindingContext.ModelType;
			var underlyingType = Nullable.GetUnderlyingType(modelType) ?? modelType;

			if (!underlyingType.IsEnum)
			{
				bindingContext.Result = ModelBindingResult.Failed();
				return Task.CompletedTask;
			}

			try
			{
				var converter = TypeDescriptor.GetConverter(underlyingType);
				var result = converter.ConvertFromString(value.Trim());

				if (!Enum.IsDefined(underlyingType, result!))
				{
					throw new InvalidEnumBadRequestException(value, underlyingType.Name);
				}

				if (modelType == underlyingType)
				{
					bindingContext.Result = ModelBindingResult.Success(result);
				}
				else
				{
					bindingContext.Result = ModelBindingResult.Success(Activator.CreateInstance(
						typeof(Nullable<>).MakeGenericType(underlyingType), result));
				}
			}
			catch (InvalidEnumBadRequestException)
			{
				throw;
			}
			catch (Exception ex) when (ex is FormatException || ex is NotSupportedException)
			{
				Log.Warning(ex, "Failed to bind enum value. Value: {Value}, EnumType: {EnumType}, ModelName: {ModelName}", value, underlyingType.Name, modelName);
				throw new InvalidEnumBadRequestException(value, underlyingType.Name);
			}

			return Task.CompletedTask;
		}
	}
}