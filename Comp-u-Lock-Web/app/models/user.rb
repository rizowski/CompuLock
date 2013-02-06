class User < ActiveRecord::Base
  # Include default devise modules. Others available are:
  # :token_authenticatable, :confirmable,
  # :lockable, :timeoutable and :omniauthable
  devise :database_authenticatable, :registerable, :token_authenticatable,
       :recoverable, :rememberable, :trackable, :validatable

  # Setup accessible (or protected) attributes for your model
  attr_accessible :username, :email, :password, :password_confirmation, :remember_me
  attr_protected :admin
  
  has_many :computer, :dependent => :destroy

  before_save :ensure_authentication_token


  # attr_accessible :title, :body

  def as_json options={}
    {
      id: id,
      username: username,
      email: email,
      computers: computer,
      created_at: created_at,
      update_at: updated_at

    }
  end
end