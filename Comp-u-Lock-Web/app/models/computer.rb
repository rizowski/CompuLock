class Computer < ActiveRecord::Base
  attr_accessible :user_id, :enviroment, :ip_address, :name

  belongs_to :user

  validates :name, presence: true
  validates :enviroment, presence:  true
  validates :user_id, presence: true

  has_many :account, dependent: :destroy

  accepts_nested_attributes_for :account

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
