class AccountProgram < ActiveRecord::Base
  	attr_accessible :account_id, :name, :open_count
  
  	validates :name, presence: true
  	validates :account_id, presence: true

	belongs_to :account

	def as_json options={}
    {
      id: id,
      account_id: account_id,
      name: name,
      open_count: open_count,
      created_at: created_at,
      update_at: updated_at

    }
  end
end
