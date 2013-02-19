class User < ActiveRecord::Base
  # Include default devise modules. Others available are:
  # :token_authenticatable, :confirmable,
  # :lockable, :timeoutable and :omniauthable
  devise :database_authenticatable, :registerable, :token_authenticatable,
       :recoverable, :rememberable, :trackable, :validatable

  # Setup accessible (or protected) attributes for your model
  attr_accessible :username, :email, :password, :password_confirmation, :remember_me,
  :computer_attributes
  attr_protected :admin
  
  has_many :computer, :dependent => :destroy
  
  before_save :ensure_authentication_token

  validates :username, uniqueness: true

  accepts_nested_attributes_for :computer

  # attr_accessible :title, :body

  def as_json options={}
    {
      id: id,
      username: username,
      email: email,
      computer_attributes: computer,
      created_at: created_at,
      update_at: updated_at

    }
  end
end