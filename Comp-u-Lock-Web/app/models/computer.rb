class Computer < ActiveRecord::Base
  attr_accessible :user_id, :enviroment, :ip_address, :name

  validates :name, :presence => true
  validates :enviroment, :presence => true
  
  has_many :account, :dependent => :destroy

  def as_json options={}
    {
      id: id,
      user_id: user_id,
      name: name,
      enviroment: enviroment,
      ip_address: ip_address,

      accounts: account,
      created_at: created_at,
      update_at: updated_at

    }
  end
end
