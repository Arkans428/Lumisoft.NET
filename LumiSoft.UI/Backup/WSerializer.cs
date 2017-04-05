using System;
using System.CodeDom;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Reflection;

namespace LumiSoft.UI.Controls
{
	/// <summary>
	/// Summary description for WSerializer.
	/// </summary>
	public class WSerializer : CodeDomSerializer 
	{		
		#region override function Deserialize

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="codeObject"></param>
		/// <returns></returns>
		public override object Deserialize(IDesignerSerializationManager manager, object codeObject)
		{
			// This is how we associate the component with the serializer.
			CodeDomSerializer baseClassSerializer = (CodeDomSerializer)manager.GetSerializer(typeof(Component),typeof(CodeDomSerializer));
			return baseClassSerializer.Deserialize(manager, codeObject);
        }

		#endregion
 
		#region override function Serialize

		/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="value"></param>
		/// <returns></returns>
        public override object Serialize(IDesignerSerializationManager manager,object value) 
		{		
			CodeDomSerializer baseClassSerializer = (CodeDomSerializer)manager.GetSerializer(typeof(Component),typeof(CodeDomSerializer));
			object codeObject = baseClassSerializer.Serialize(manager,value);

			if(value.GetType().GetInterface("IWSerializer") == null){
				System.Windows.Forms.MessageBox.Show("Must not never reach here:" + value.GetType().ToString());
				return codeObject;				
			}

			MethodInfo mInf = value.GetType().GetInterface("IWSerializer").GetMethod("ShouldSerialize");
												
            if(codeObject is CodeStatementCollection){
                CodeStatementCollection statements = (CodeStatementCollection)codeObject;
 			
				//--- loop through all statements
				int count = statements.Count;
				for(int i=0;i<count;i++){
					CodeStatement st = statements[i];

					if(st is CodeAssignStatement){
						CodeAssignStatement cAssign = (CodeAssignStatement)st;
                    
						// If left is eg. 'this.BorderColor'
						if(cAssign.Left is CodePropertyReferenceExpression){
						/*  if(cAssign.Right is CodeCastExpression){
								CodeCastExpression c = (CodeCastExpression)cAssign.Right;
								if(c.Expression is CodeMethodInvokeExpression){
									CodeMethodInvokeExpression mI = (CodeMethodInvokeExpression)c.Expression;
									if(mI.Method.TargetObject is CodeVariableReferenceExpression){
										CodeVariableReferenceExpression vR = (CodeVariableReferenceExpression)mI.Method.TargetObject;
										System.Windows.Forms.MessageBox.Show(vR.);
									}
								}
							}*/

							string propertyName = ((CodePropertyReferenceExpression)cAssign.Left).PropertyName;							
							//--- Check if we need to serialize property.
							if(!(bool)mInf.Invoke(value,new object[]{propertyName})){				
								statements.Remove(st);
								count--;
								i--;
							}
						}
					}
				}
            }
         
			return codeObject;
        }
	
		#endregion


	/*	/// <summary>
		/// 
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="resourceName"></param>
		/// <param name="value"></param>
		protected new void SerializeResource(IDesignerSerializationManager manager,string resourceName,object value)
		{
			System.Windows.Forms.MessageBox.Show(resourceName);
			//base.SerializeResource(manager,resourceName,value);
		}*/

	}
}
