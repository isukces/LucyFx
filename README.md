LucyFx
======

Extensions to NancyFx framework


### Processing POST form data example

The following example shows how to:

* preprocess POST data, i.e. removing unnecessary spaces from GroupName field before model binding and validating 
* dealing with ModelBindingException


```c#
        private dynamic _PostEdit(dynamic parameters)
        {
            try
            {
                Request.ProcessFormFor<ModelClass>(i => i.GroupName)
                    .NormalizeString();
                var model = this.BindAndValidate<ModelClass>();
                if (!ModelValidationResult.IsValid)
                    return View["Edit", model];
                // ... process valid data here
            }
            catch (ModelBindingException ex)
            {
                this.AttachException(ex);
                return View["Edit", new ModelClass()];
            }
        }
```
