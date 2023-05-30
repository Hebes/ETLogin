
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class DlgRoleViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.RectTransform EGBackGroundRectTransform
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EGBackGroundRectTransform == null )
     			{
		    		this.m_EGBackGroundRectTransform = UIFindHelper.FindDeepChild<UnityEngine.RectTransform>(this.uiTransform.gameObject,"EGBackGround");
     			}
     			return this.m_EGBackGroundRectTransform;
     		}
     	}

		public UnityEngine.UI.Button E_CreatRoleButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CreatRoleButton == null )
     			{
		    		this.m_E_CreatRoleButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"EGBackGround/E_CreatRole");
     			}
     			return this.m_E_CreatRoleButton;
     		}
     	}

		public UnityEngine.UI.Image E_CreatRoleImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CreatRoleImage == null )
     			{
		    		this.m_E_CreatRoleImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EGBackGround/E_CreatRole");
     			}
     			return this.m_E_CreatRoleImage;
     		}
     	}

		public UnityEngine.UI.Button E_DelectRoleButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_DelectRoleButton == null )
     			{
		    		this.m_E_DelectRoleButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"EGBackGround/E_DelectRole");
     			}
     			return this.m_E_DelectRoleButton;
     		}
     	}

		public UnityEngine.UI.Image E_DelectRoleImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_DelectRoleImage == null )
     			{
		    		this.m_E_DelectRoleImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EGBackGround/E_DelectRole");
     			}
     			return this.m_E_DelectRoleImage;
     		}
     	}

		public UnityEngine.UI.Button E_StartGameButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_StartGameButton == null )
     			{
		    		this.m_E_StartGameButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"EGBackGround/E_StartGame");
     			}
     			return this.m_E_StartGameButton;
     		}
     	}

		public UnityEngine.UI.Image E_StartGameImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_StartGameImage == null )
     			{
		    		this.m_E_StartGameImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EGBackGround/E_StartGame");
     			}
     			return this.m_E_StartGameImage;
     		}
     	}

		public UnityEngine.UI.InputField EInputFieldNameInputField
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EInputFieldNameInputField == null )
     			{
		    		this.m_EInputFieldNameInputField = UIFindHelper.FindDeepChild<UnityEngine.UI.InputField>(this.uiTransform.gameObject,"EGBackGround/EInputFieldName");
     			}
     			return this.m_EInputFieldNameInputField;
     		}
     	}

		public UnityEngine.UI.Image EInputFieldNameImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EInputFieldNameImage == null )
     			{
		    		this.m_EInputFieldNameImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EGBackGround/EInputFieldName");
     			}
     			return this.m_EInputFieldNameImage;
     		}
     	}

		public UnityEngine.UI.Text ERoleNameText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_ERoleNameText == null )
     			{
		    		this.m_ERoleNameText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"EGBackGround/ERoleName");
     			}
     			return this.m_ERoleNameText;
     		}
     	}

		public UnityEngine.UI.Image EContentImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EContentImage == null )
     			{
		    		this.m_EContentImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"Scroll View/Viewport/EContent");
     			}
     			return this.m_EContentImage;
     		}
     	}

		public UnityEngine.UI.Button ERoleItemButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_ERoleItemButton == null )
     			{
		    		this.m_ERoleItemButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"Scroll View/Viewport/EContent/ERoleItem");
     			}
     			return this.m_ERoleItemButton;
     		}
     	}

		public UnityEngine.UI.Image ERoleItemImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_ERoleItemImage == null )
     			{
		    		this.m_ERoleItemImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"Scroll View/Viewport/EContent/ERoleItem");
     			}
     			return this.m_ERoleItemImage;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_EGBackGroundRectTransform = null;
			this.m_E_CreatRoleButton = null;
			this.m_E_CreatRoleImage = null;
			this.m_E_DelectRoleButton = null;
			this.m_E_DelectRoleImage = null;
			this.m_E_StartGameButton = null;
			this.m_E_StartGameImage = null;
			this.m_EInputFieldNameInputField = null;
			this.m_EInputFieldNameImage = null;
			this.m_ERoleNameText = null;
			this.m_EContentImage = null;
			this.m_ERoleItemButton = null;
			this.m_ERoleItemImage = null;
			this.uiTransform = null;
		}

		private UnityEngine.RectTransform m_EGBackGroundRectTransform = null;
		private UnityEngine.UI.Button m_E_CreatRoleButton = null;
		private UnityEngine.UI.Image m_E_CreatRoleImage = null;
		private UnityEngine.UI.Button m_E_DelectRoleButton = null;
		private UnityEngine.UI.Image m_E_DelectRoleImage = null;
		private UnityEngine.UI.Button m_E_StartGameButton = null;
		private UnityEngine.UI.Image m_E_StartGameImage = null;
		private UnityEngine.UI.InputField m_EInputFieldNameInputField = null;
		private UnityEngine.UI.Image m_EInputFieldNameImage = null;
		private UnityEngine.UI.Text m_ERoleNameText = null;
		private UnityEngine.UI.Image m_EContentImage = null;
		private UnityEngine.UI.Button m_ERoleItemButton = null;
		private UnityEngine.UI.Image m_ERoleItemImage = null;
		public Transform uiTransform = null;
	}
}
