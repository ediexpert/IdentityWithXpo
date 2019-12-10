using DevExpress.Web.Mvc;
using System.Web.Mvc;
using System;
using DevExpress.Xpo.Metadata;
using System.ComponentModel;

public class XpoModelBinder : DevExpressEditorsBinder
{
    protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
    {

        IXpoController xpoController = controllerContext.Controller as IXpoController;
        if (xpoController == null) throw new InvalidOperationException("The controller does not support IXpoController interface");
        XPClassInfo classInfo = xpoController.XpoSession.GetClassInfo(modelType);
        ModelBindingContext keyPropertyBindingContext = new ModelBindingContext()
        {
            ModelMetadata = bindingContext.PropertyMetadata[classInfo.KeyProperty.Name],
            ModelName = classInfo.KeyProperty.Name,
            ModelState = bindingContext.ModelState,
            ValueProvider = bindingContext.ValueProvider
        };
        PropertyDescriptorCollection properties = GetModelProperties(controllerContext, bindingContext);
        PropertyDescriptor keyProperty = properties.Find(classInfo.KeyProperty.Name, false);
        IModelBinder keyPropertyBinder = Binders.GetBinder(keyProperty.PropertyType);
        object keyValue = GetPropertyValue(controllerContext, keyPropertyBindingContext, keyProperty, keyPropertyBinder);
        if (keyValue == null)
            return classInfo.CreateNewObject(xpoController.XpoSession);
        else return xpoController.XpoSession.GetObjectByKey(classInfo, keyValue);
    }
}