class Restriction < ActiveRecord::Base
	attr_accessible :account_id, :day_attributes

	has_many :day, :dependent => :destroy

	accepts_nested_attributes_for :day
	
  	def as_json options={}
    {
      id: id,
      account_id: account_id,

      day_attributes: day,

      created_at: created_at,
      update_at: updated_at
    }
  end
end
