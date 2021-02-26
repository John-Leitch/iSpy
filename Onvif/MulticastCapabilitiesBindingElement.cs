using System;
using System.ServiceModel.Channels;

namespace iSpyApplication.Onvif
{
    public class MulticastCapabilitiesBindingElement : BindingElement, IBindingMulticastCapabilities
    {
        private readonly bool _isMulticast;
        public MulticastCapabilitiesBindingElement(bool isMulticast) => _isMulticast = isMulticast;
        public override T GetProperty<T>(BindingContext context) => typeof(T) == typeof(IBindingMulticastCapabilities)
                ? (T)(object)this
                : context == null ? throw new ArgumentNullException(nameof(context)) : context.GetInnerProperty<T>();
        bool IBindingMulticastCapabilities.IsMulticast => _isMulticast;

        public override BindingElement Clone() => this;
    }
}