using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace PepijnWillekens.ManagerSystem {
	public class ServiceTypeInfo {

		public List<FieldInfo> injectFields = new List<FieldInfo>();

		public List<FieldInfo> tryInjectFields = new List<FieldInfo>();

	}
}
